using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ISystemConfiguration
    {
        MDataRow InitDataRow();

        MDataRow GetEntity(int dicId, bool bCache = false);

        MDataRow GetEntityByDicCode(string sDicCode, bool bCache = false);

        MDataTable GetList(string companyCode,string sDicCode, int pageIndex, int pageSize, ref int recordCount, ref int pageCount);

        ExeMsgInfo Insert(MDataRow dataRow);

        ExeMsgInfo UpdateByDicId(MDataRow dataRow);

        ExeMsgInfo UpdateByDicCode(MDataRow dataRow);

        ExeMsgInfo DeleteByDicId(int dicId);

        ExeMsgInfo DeleteByDicCode(string sDicCode);

        MDataTable GetAll(string sDicCode,string companyCode, bool bCache = false);

        /// <summary>
        /// 前端
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        MDataTable GetWithCompanyCodeAndParentCode(string companyCode,string parentCode);
    }
}
