/*=============================================
	주의 사항
=============================================*/	
--

/*=============================================
	DROP
=============================================*/	
/*-----------------------------------
	View DROP
-----------------------------------*/

/*-----------------------------------
	TABLE DROP
-----------------------------------*/
DROP TABLE IF EXISTS debug_logger;
DROP TABLE IF EXISTS debug_http_log;

/*-----------------------------------
	FUNCTION DROP
-----------------------------------*/
--DROP FUNCTION IF EXISTS newid();

/*=============================================
	FUNCTION Create
=============================================*/
--CREATE OR REPLACE FUNCTION newid() RETURNS uuid stable language sql as 'SELECT md5(random()::text || clock_timestamp()::text)::uuid';

/*=============================================
	TABLE
=============================================*/	

------------------------------
-- 개발로그
------------------------------
CREATE TABLE debug_logger(
	log_id varchar(36) NOT NULL DEFAULT newid(),
	logger varchar(200) NOT NULL,
	date_log timestamp with time zone NOT NULL,
	log_level varchar(10) NOT NULL,
	message varchar(8000) NULL,
	stacktrace varchar(8000) NULL,
	machine_name varchar(100) NULL,
	PRIMARY KEY (log_id)
);

COMMENT ON TABLE debug_logger IS '개발로그';
COMMENT ON COLUMN debug_logger.log_id IS '로그아이디 : 자동생성';
COMMENT ON COLUMN debug_logger.logger IS '로거명';
COMMENT ON COLUMN debug_logger.date_log IS '로그일시';
COMMENT ON COLUMN debug_logger.log_level IS '로그레벨';
COMMENT ON COLUMN debug_logger.message IS 'message';
COMMENT ON COLUMN debug_logger.stacktrace IS 'stacktrace';
COMMENT ON COLUMN debug_logger.machine_name IS '장비명';

CREATE INDEX idx_logger_dt ON debug_logger USING btree (date_log DESC NULLS FIRST);

------------------------------
-- 웹접속로그
------------------------------

CREATE TABLE debug_http_log(
	traking_id varchar(50) NOT NULL,
	user_id varchar(100) NULL REFERENCES com_user(user_id),
	ip_addr varchar(15) NULL,
	user_agent varchar(500) NULL,
	referer varchar(500) NULL,
	method varchar(50) NULL,
	path varchar(500) NULL,
	query_string varchar(500) NULL,
	protocol varchar(50) NULL,
	request_header varchar(8000) NULL,
	request_body text NULL,
	date_request timestamp with time zone NULL,
	status_code int NOT NULL DEFAULT 0,
	response_header varchar(8000) NULL,
	response_body text NULL,
	date_response timestamp with time zone NULL,
	execute_time varchar(50) NOT NULL DEFAULT 0,
	machine_name varchar(100) NULL,
	PRIMARY KEY (traking_id)
);

COMMENT ON TABLE debug_http_log IS '웹접속로그';

CREATE INDEX idx_http_dt ON debug_http_log USING btree (date_request DESC NULLS FIRST);

/*=============================================
	VIEW
=============================================*/	


/*== End Of Document ========================*/	