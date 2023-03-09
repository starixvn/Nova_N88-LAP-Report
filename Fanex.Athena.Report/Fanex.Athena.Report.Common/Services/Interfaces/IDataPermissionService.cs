using Fanex.Athena.Report.Common.Model.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Common.Services.Interfaces
{
    public interface IDataPermissionService
    {
        void ClearDataByPermission<T>(T data);
    }
}