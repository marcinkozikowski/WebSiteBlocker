using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSiteBlocker.Itrefaces;

namespace WebSiteBlocker.Classes
{
    class Output : IOutputObservable
    {
        private List<IOutputObserver> _listaObserwatorow = new List<IOutputObserver>();

        public string _message { get; set; }

        public void dodajObserwatora(IOutputObserver o)
        {
            _listaObserwatorow.Add(o);
        }

        public void Notify()
        {
            foreach (var item in _listaObserwatorow)
            {
                if (_message != null)
                {
                    item.Update();
                }
            }
        }

        public void usunObserwator(IOutputObserver o)
        {
            _listaObserwatorow.Remove(o);
        }
        public void setMessage(string message)
        {
            _message = message;
        }
        public string getMessage()
        {
            return _message;
        }

        public void publishMessage(string msg)
        {
            _message = msg;
            Notify();
        }
    }
}
