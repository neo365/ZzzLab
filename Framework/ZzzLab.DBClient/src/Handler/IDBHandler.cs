using System;
using System.Collections.Generic;
using System.Data;

namespace ZzzLab.Data
{
    public interface IDBHandler : IDisposable
    {
        bool IsDebug { set; get; }

        /// <summary>
        /// 서버타입
        /// </summary>
        DataBaseType ServerType { get; }

        /// <summary>
        /// 서버그룹
        /// </summary>
        string Group { get; }

        /// <summary>
        /// 서버명(코드)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 서버별칭
        /// </summary>
        string AliasName { get; }

        /// <summary>
        /// 연결문자열
        /// </summary>
        string ConnectionString { get; }

        //object CreateDBConnection();

        //object CreateDBCommand();

        //void CrearDBConnection(object conn);

        /// <summary>
        /// Database Version을 가져온다
        /// </summary>
        /// <returns></returns>
        string GetVersion();

        /// <summary>
        /// 연결테스트
        /// </summary>
        /// <returns>연결성공 여부</returns>
        bool ConnectionTest();

        /// <summary>
        /// Database Select
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <returns></returns>
        DataTable Select(string commandText);

        /// <summary>
        /// Database Select
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <param name="parameters">파라미터</param>
        /// <returns></returns>
        DataTable Select(string commandText, QueryParameterCollection parameters);

        /// <summary>
        /// Database Select
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DataTable Select(Query query);

        /// <summary>
        /// Database Select후 처음의 하나만 가져온다. top 1 또는 limit 1 과 같은 효과
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <returns></returns>
        DataRow SelectRow(string commandText);

        /// <summary>
        /// Database Select후 처음의 하나만 가져온다. top 1 또는 limit 1 과 같은 효과
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <param name="parameters">파라미터</param>
        /// <returns></returns>
        DataRow SelectRow(string commandText, QueryParameterCollection parameters);

        /// <summary>
        /// Database Select후 처음의 하나만 가져온다. top 1 또는 limit 1 과 같은 효과
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DataRow SelectRow(Query query);

        /// <summary>
        /// Database Select후 맨앞의 하나만 가져온다.
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <returns></returns>
        object SelectValue(string commandText);

        /// <summary>
        /// Database Select후 맨앞의 하나만 가져온다.
        /// </summary>
        /// <param name="commandText">쿼리문</param>
        /// <param name="parameters">변수</param>
        /// <returns></returns>

        object SelectValue(string commandText, QueryParameterCollection parameters);

        /// <summary>
        /// Database Select후 맨앞의 하나만 가져온다.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        object SelectValue(Query query);

        /// <summary>
        /// 쿼리문을 실행한다. 쿼리가 여러개일경우 트랜젝션 처리된다.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        int Excute(QueriesCollection queries);

        /// <summary>
        /// 쿼리문을 실행한다. 쿼리가 여러개일경우 트랜젝션 처리된다.
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        int Excute(params Query[] queries);

        /// <summary>
        /// 쿼리문을 실행한다.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        int Excute(string commandText);

        int Excute(string commandText, QueryParameterCollection parameters);

        /// <summary>
        /// Support Vacuum
        /// </summary>
        /// <param name="options"></param>
        void Vacuum(IDictionary<string, string> options = null);

        /// <summary>
        /// BulkCopy / Bulk Insert 지원
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        bool BulkCopy(DataTable table);

        /// <summary>
        ///  BulkCopy 지원
        /// </summary>
        /// <param name="tableName">대상테이블</param>
        /// <param name="filePath">파일경로</param>
        /// <param name="offset">건너뛸 라인</param>
        /// <returns></returns>
        bool BulkCopyFromFile(string tableName, string filePath, int offset = 0);

        /// <summary>
        /// 쿼리를 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        string GetQuery(string section, string label);

        string GetQuery(string section, string label, QueryParameterCollection parameters);

        string MakePagingQuery(string query, int pageNum, int pageSize);
    }
}