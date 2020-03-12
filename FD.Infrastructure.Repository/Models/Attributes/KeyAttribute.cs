using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute() { }

        public KeyAttribute(string oracleSequence)
        {
            OracleSequence = oracleSequence;
        }

        /// <summary>
        /// Oracle Sequence
        /// </summary>
        public string OracleSequence { get; set; }
    }
}
