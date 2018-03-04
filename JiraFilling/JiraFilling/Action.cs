using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraFilling
{
    public enum ActionType { Done, Todo, Defect };
    public class Action
    {
        public string Texte { get; set; }
        public string Titre { get; set; }
        public ActionType Type { get; set; }
        public double Temps { get; set; }
        public bool CustomerCharge { get; set; }
    }
}
