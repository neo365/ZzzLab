using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using ZzzLab.Data.Models;

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
        /// 연결문자열
        /// </summary>
        string ConnectionString { get; }

        IDbConnection CreateDBConnection();

        IDbCommand CreateDBCommand();

        void CrearDBConnection(IDbConnection conn);

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
        int Excute(QueryCollection queries);

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
        /// 쿼리를 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        string GetQuery(string section, string label);

        /// <summary>
        /// 쿼리를 가져온다
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetQuery(string section, string label, Hashtable parameters);

        /// <summary>
        /// 쿼리를 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        string GetQuery(string section, string label, string search);

        /// <summary>
        /// 쿼리를 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="label"></param>
        /// <param name="search"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        string GetQuery(string section, string label, string search, string order);

        /// <summary>
        /// View를 포함한 테이블 목록을 가져온다.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TableInfo> GetTableList();

        /// <summary>
        /// 지정된 테이블 정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        TableInfo GetTableInfo(string schemaName, string databaseName, string tableName);

        /// <summary>
        /// 지정된 테이블 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        IEnumerable<TableColomn> GetTableColumns(TableInfo tableInfo);

        /// <summary>
        /// 지정된 테이블 컬럼정보를 가져온다.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IEnumerable<TableColomn> GetTableColumns(string schemaName, string databaseName, string tableName);
    }
}