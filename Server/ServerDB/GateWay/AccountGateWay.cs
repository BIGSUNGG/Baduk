using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDB
{
    public class AccountGateWay
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Score { get; set; }

        // DB 접속 문자열
        private static string connectionString = "Data Source=DESKTOP-1F4264U\\SQLEXPRESS;Initial Catalog=Omok;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public bool Select(string inName, string inPassword)
        {
            // 성공 여부를 반환하는 Query 설정
            Func<SqlCommand, object> query = command => 
            {
                // SELECT문 설정
                command.CommandText = "SELECT TOP (1) id, name, password, score FROM Account WHERE name=@name";
                command.Parameters.Add(new SqlParameter("@name", inName));

                // SELECT문 실행 후 읽기 시도
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == false)
                    return false;

                // 읽은 값 불러오기
                int index = 0;
                int id = reader.GetInt32(index++);
                string name = reader.GetString(index++);
                string password = reader.GetString(index++);
                int score = reader.GetInt32(index++);
                reader.Close();

                if (password != inPassword)
                {
                    Trace.WriteLine("Wrong password");
                    return false;
                }

                this.Id = id;
                this.Name = name;
                this.Password = password;
                this.Score = score;

                return true;
            };

            return (bool)DoQuery(query);
        }

        public bool Insert()
        {
            // 성공 여부를 반환하는 Query 설정
            Func<SqlCommand, object> query = command =>
            {
                // INSERT문 설정
                command.CommandText = "INSERT INTO Account (name, password, score) VALUES(@name, @password, @score); SELECT SCOPE_IDENTITY();";

                command.Parameters.Add(new SqlParameter("@name", Name));
                command.Parameters.Add(new SqlParameter("@password", Password));
                command.Parameters.Add(new SqlParameter("@score", Score));

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == false)
                    return false;

                int index = 0;
                Id = (int)reader.GetDecimal(index++);
                reader.Close();

                return true;
            };

            return (bool)DoQuery(query);
        }

        public int Update()
        {
            // 성공 여부를 반환하는 Query 설정
            Func<SqlCommand, object> query = command =>
            {
                // UPDATE문 설정
                command.CommandText = "UPDATE Account SET name=@name, password=@password, score=@score WHERE id=@id";

                command.Parameters.Add(new SqlParameter("@name", Name));
                command.Parameters.Add(new SqlParameter("@password", Password));
                command.Parameters.Add(new SqlParameter("@score", Score));
                command.Parameters.Add(new SqlParameter("@id", Id));

                return command.ExecuteNonQuery();
            };

            return (int)DoQuery(query);
        }

        public int Delete()
        {
            // 성공 여부를 반환하는 Query 설정
            Func<SqlCommand, object> query = command =>
            {
                // DELETE 설정
                command.CommandText = "DELETE FROM Account WHERE id=@id";
                command.Parameters.Add(new SqlParameter("@id", Id));

                return command.ExecuteNonQuery();
            };

            return (int)DoQuery(query);
        }

        /// <summary>
        /// 계정의 점수를 기준으로 순위를 구함
        /// </summary>
        /// <returns></returns>
        public int GetRank()
        {
            // 성공 여부를 반환하는 Query 설정
            Func<SqlCommand, object> query = command =>
            {
                // INSERT문 설정
                command.CommandText = @"
WITH RankInfo AS 
(
	SELECT id, name, password, score, CAST(ROW_NUMBER() OVER (ORDER BY score DESC) AS INT) AS rank
	FROM Account
)
SELECT TOP (1) rank
FROM RankInfo
WHERE id = @id
";

                command.Parameters.Add(new SqlParameter("@id", Id));

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == false)
                    return -1;

                int index = 0;
                int rank = reader.GetInt32(index++);
                reader.Close();

                return rank;
            };

            return (int)DoQuery(query);
        }

        /// <summary>
        /// 점수를 기준으로 순위가 top 위부터 bottom 위까지의 계정 찾기
        /// </summary>
        /// <param name="leftRank">1이상의 수</param>
        /// <param name="rightRank">leftRank이상의 수</param>
        /// <returns>leftRank ~ rightRank 까지의 유저들</returns>
        public static List<AccountGateWay> GetTopRank(int leftRank, int rightRank)
        {
            Func<SqlCommand, List<AccountGateWay>> query = command =>
            {
                List<AccountGateWay> result = new List<AccountGateWay>();
                // SELECT문 설정
                command.CommandText = @"
WITH RankInfo AS 
(
	SELECT id, name, password, score, ROW_NUMBER() OVER (ORDER BY score DESC) AS rank
	FROM Account
)
SELECT id, name, password, score, rank
FROM RankInfo
WHERE Rank Between @leftRank And @rightRank
                ";
                command.Parameters.Add(new SqlParameter("@leftRank", leftRank));
                command.Parameters.Add(new SqlParameter("@rightRank", rightRank));

                // SELECT문 실행 후 읽기 시도
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i <= rightRank - leftRank; i++)
                {
                    if (reader.Read() == false)
                        return null;

                    // 읽은 값 불러오기
                    int index = 0;

                    AccountGateWay account = new AccountGateWay();

                    int id = reader.GetInt32(index++);
                    string name = reader.GetString(index++);
                    string password = reader.GetString(index++);
                    int score = reader.GetInt32(index++);

                    account.Id = id;
                    account.Name = name;
                    account.Password = password;
                    account.Score = score;
                    result.Add(account);
                }
                reader.Close();

                return result;
            };

            return (List<AccountGateWay>)DoQuery(query);
        }

        /// <summary>
        /// 데이터베이스 연동 후 query 함수 실행
        /// </summary>
        /// <param name="query">DB 연결 후 실행할 행동</param>
        /// <returns>query의 반환값</returns>
        static private object DoQuery(Func<SqlCommand, object> query)
        {
            // 데이터베이스 연결
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // DB 서버 접속 시도
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                object result = query.Invoke(command);

                // DB 서버 접속 종료
                connection.Close();

                return result;
            }
        }
    }
}

