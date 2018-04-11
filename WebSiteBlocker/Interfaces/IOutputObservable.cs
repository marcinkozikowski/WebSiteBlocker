using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteBlocker.Itrefaces
{
    interface IOutputObservable
    {
        string _message { get; set; }
        void dodajObserwatora(IOutputObserver o);
        void usunObserwator(IOutputObserver o);
        void publishMessage(string msg);
        void Notify();
    }
}
