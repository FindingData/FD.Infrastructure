using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Infrastructure.Repository.Models
{
    /// <summary>
    /// Specifies that this is a computed column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
    }
}
