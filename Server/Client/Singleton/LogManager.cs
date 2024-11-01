using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class LogManager
    {
        private object _lock = new object();
        private Queue<string> messages = new Queue<string>();

        public static LogManager _instance;
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
                return result;
            }
        }
    }
}
