using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class LogManager
    {
        public object _lock = new object();
        private Queue<string> messages = new Queue<string>();

        private static LogManager _instance;
        public static LogManager Instance 
        {
            get 
            { 
                if (_instance == null)
                    _instance = new LogManager();

                return _instance;
            }
        }

        private LogManager() { }

        public void PushMessage(string message)
        {
            lock(_lock)
            {
                messages.Enqueue(message);
            }
        }

        public Queue<string> PopMessage()
        {
            lock (_lock)
            {
                Queue<string> result = messages;
                messages = new Queue<string>();

                if(result.Count > 0)
                {
                    int a = 0;
                }
                return result;
            }
        }
    }
}
