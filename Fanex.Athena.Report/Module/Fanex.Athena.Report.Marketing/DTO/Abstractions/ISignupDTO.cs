using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public interface ISignupDTO<T>
    {
        string SignUpSource { get; set; }
        string SignupUrl { get; set; }
        bool NoGrouping { get; set; }
        IEnumerable<T> Childs { get; set; }
    }
}