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
DROP VIEW IF EXISTS vw_ota;
DROP VIEW IF EXISTS vw_user;
DROP VIEW IF EXISTS vw_login_log;
DROP VIEW IF EXISTS vw_user_authrole;
DROP VIEW IF EXISTS vw_user_authrole_primary;

/*-----------------------------------
	Table DROP
-----------------------------------*/
/* Debug */
DROP TABLE IF EXISTS debug_http_log;
DROP TABLE IF EXISTS debug_logger;

/* Auth */
DROP TABLE IF EXISTS com_user_authrole;
DROP TABLE IF EXISTS com_authrole;

/* Commonm */
DROP TABLE IF EXISTS com_login_log;
DROP TABLE IF EXISTS com_ota;
DROP TABLE IF EXISTS com_user;
DROP TABLE IF EXISTS com_dept;
DROP TABLE IF EXISTS com_company;

/*-----------------------------------
	FUNCTION DROP
-----------------------------------*/
DROP FUNCTION IF EXISTS newid();

/*=============================================
	FUNCTION Create
=============================================*/
CREATE OR REPLACE FUNCTION newid() RETURNS uuid stable language sql as 'SELECT md5(random()::text || clock_timestamp()::text)::uuid';


CREATE OR REPLACE FUNCTION LastIndexOf(text,char) 
RETURNS integer LANGUAGE SQL AS $$  
select LENGTH($1) - strpos(reverse($1),$2)
$$;

/*=============================================
	TABLE
=============================================*/	
------------------------------
-- 회사정보
------------------------------

CREATE TABLE com_company (
	company_id varchar(50) NOT NULL DEFAULT newid(),
	company_name varchar(200),
    parent_id varchar(50),
    used_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,    
	when_synced timestamp with time zone,
	PRIMARY KEY (company_id)
);

COMMENT ON TABLE com_company IS '회사정보';

/* Default Value */
INSERT INTO com_company ( company_id, company_name, parent_id, used_yn, memo) 
VALUES 
  ('00000000-0000-0000-0000-000000000000',  'ZzzLab', null, 'Y', null)
;
------------------------------
-- 부서정보
------------------------------

CREATE TABLE com_dept (
	dept_id varchar(50) NOT NULL DEFAULT newid(),
	dept_name varchar(200),
    company_id varchar(36) NOT NULL REFERENCES com_company(company_id),
    parent_id varchar(36),
    used_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,  
	when_synced timestamp with time zone,
	PRIMARY KEY (dept_id)
);	

COMMENT ON TABLE com_dept IS '부서정보';

/* Default Value */
INSERT INTO com_dept ( company_id, dept_id, dept_name, parent_id, used_yn, memo) 
VALUES 
  ('00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000',  '개발팀' , null, 'Y', null)
;

------------------------------
-- 사용자정보
------------------------------
CREATE TABLE com_user (
	user_id varchar(50) NOT NULL,
	user_pwd varchar(500),
	user_name varchar(100),
	nick_name varchar(100),
	company_id varchar(50) REFERENCES com_company(company_id),
	dept_id varchar(50) REFERENCES com_dept(dept_id),
	email varchar(200),
	mobile varchar(50),
	status varchar(50),
	position varchar(50),
	location varchar(100),
	login_yn char(1) NOT NULL DEFAULT 'Y',
	used_yn char(1) NOT NULL DEFAULT 'Y',
	memo varchar(8000),
	when_created timestamp with time zone NOT NULL DEFAULT now(),
	when_changed timestamp with time zone,
	when_pwd_changed date,
	when_expired date NOT NULL DEFAULT '9999-12-31',
	api_key varchar(36) NOT NULL DEFAULT newid(),
	when_synced timestamp with time zone,
	PRIMARY KEY (user_id)
);

COMMENT ON TABLE com_user IS '사용자 정보';
COMMENT ON COLUMN com_user.user_id IS '아이디';
COMMENT ON COLUMN com_user.user_pwd IS '비밀번호';
COMMENT ON COLUMN com_user.user_name IS '이름';
COMMENT ON COLUMN com_user.nick_name IS '별칭';
COMMENT ON COLUMN com_user.company_id IS '회사코드';
COMMENT ON COLUMN com_user.dept_id IS '부서코드';
COMMENT ON COLUMN com_user.email IS '이메일';
COMMENT ON COLUMN com_user.mobile IS '휴대폰';
COMMENT ON COLUMN com_user.login_yn IS '로그인가능여부 (Y/N)';
COMMENT ON COLUMN com_user.used_yn IS '사용여부 (Y/N)';
COMMENT ON COLUMN com_user.memo IS '메모';
COMMENT ON COLUMN com_user.when_created IS '생성일';
COMMENT ON COLUMN com_user.when_changed IS '변경일';
COMMENT ON COLUMN com_user.when_pwd_changed IS '패스워드 변경일';
COMMENT ON COLUMN com_user.when_expired IS '사용만료일';
COMMENT ON COLUMN com_user.api_key IS '사용자 고유 접근 코드';
COMMENT ON COLUMN com_user.when_synced IS '사용자 동기화일시';

CREATE INDEX idx_com_user_name ON com_user USING btree (user_name ASC) TABLESPACE pg_default;
	
/* Default Value */
INSERT INTO com_user ( user_id, user_pwd, user_name, company_id, dept_id, login_yn, memo) 
VALUES 
  ('admin',  null, '관리자', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'N', '시스템운영계정')
, ('devel',  null, '개발자', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'N', '개발자계정')
, ('SYSTEM', null, '시스템', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'N', '시스템운영계정. 로그인안됨')
, ('neo365', null, '김영동', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'Y', null)
;

------------------------------
-- OTP / SSO
------------------------------
CREATE TABLE com_ota (
	ota_id varchar(36) NOT NULL DEFAULT newid(),
	ota_key varchar(1000) NOT NULL,
	user_id varchar(50) NOT NULL REFERENCES com_user(user_id),
	client_key varchar(36) NOT NULL,
	when_expired timestamp with time zone NOT NULL DEFAULT current_timestamp + (5 * interval '1 minute'),
	when_used timestamp with time zone,
	used_yn char(1) NOT NULL DEFAULT 'Y',
	memo varchar(8000),
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone, 
	PRIMARY KEY (ota_id)
);

------------------------------
-- 권한정보
------------------------------

CREATE TABLE com_authrole (
	auth_role varchar(50) NOT NULL,
	auth_name varchar(100),
	auth_desc varchar(8000),
    auth_rule jsonb,
    used_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,  	
	PRIMARY KEY (auth_role)
);

COMMENT ON TABLE com_authrole IS '권한정보';

INSERT INTO com_authrole ( auth_role, auth_name, auth_desc, auth_rule) 
VALUES 
  ('ADMIN_ROLE',  '관리자', null, null)
, ('SYSTEM_ROLE', '시스템', null, null)
, ('USER_ROLE',   '사용자', null, null)
;

------------------------------
-- 사용자 권한정보
------------------------------

CREATE TABLE com_user_authrole (
    user_id varchar(50) NOT NULL  REFERENCES com_user(user_id),
	auth_role varchar(50) NOT NULL  REFERENCES com_authrole(auth_role),
	auth_rule jsonb,
    auth_order int NOT NULL DEFAULT '0',
    used_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,  	
	PRIMARY KEY (user_id, auth_role)
);

COMMENT ON TABLE com_user_authrole IS '사용자 권한정보';

INSERT INTO com_user_authrole ( user_id, auth_role) 
VALUES 
  ('admin', 'ADMIN_ROLE')
, ('devel', 'SYSTEM_ROLE')
, ('SYSTEM', 'SYSTEM_ROLE')
, ('neo365','USER_ROLE')
;

------------------------------
-- 사용자접속로그
------------------------------

CREATE TABLE com_login_log (
	uuid varchar(36) NOT NULL DEFAULT newid(),
	user_id varchar(50) NOT NULL REFERENCES com_user(user_id),
	login_ip varchar(15) NOT NULL,	
	user_agent varchar(1000) NOT NULL,
	login_type varchar(100) NOT NULL,
	user_status varchar(10) NOT NULL DEFAULT 'Open',
	memo varchar(8000),
	when_expired timestamp with time zone NOT NULL DEFAULT current_timestamp + (1 * interval '1 minute'),
	watch_url varchar(4000),
	refresh_token varchar(100),
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	date_updated timestamp with time zone,
	PRIMARY KEY (uuid)
);

COMMENT ON TABLE com_login_log IS '로그인 로그';
COMMENT ON COLUMN com_login_log.uuid IS 'uuid';
COMMENT ON COLUMN com_login_log.user_id IS '아이디';
COMMENT ON COLUMN com_login_log.login_ip IS '접속IP';
COMMENT ON COLUMN com_login_log.user_agent IS '접속정보';
COMMENT ON COLUMN com_login_log.login_type IS '로그인방법';
COMMENT ON COLUMN com_login_log.user_status IS '현재상태';
COMMENT ON COLUMN com_login_log.memo IS '메모';
COMMENT ON COLUMN com_login_log.when_expired IS '인증만료일';
COMMENT ON COLUMN com_login_log.watch_url IS '최종접속url';
COMMENT ON COLUMN com_login_log.refresh_token IS '갱신토큰';
COMMENT ON COLUMN com_login_log.date_inserted IS '접속일시';
COMMENT ON COLUMN com_login_log.date_updated IS '갱신일시';

CREATE INDEX idx_sso_dt ON com_login_log USING btree (date_inserted DESC NULLS FIRST);


------------------------------
-- 개발로그
------------------------------
CREATE TABLE debug_logger(
	log_id varchar(36) NOT NULL DEFAULT newid(),
	logger varchar(200) NULL,
	date_log timestamp with time zone NULL,
	log_level varchar(10) NULL,
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
COMMENT ON COLUMN debug_logger.message IS '로그내용';
COMMENT ON COLUMN debug_logger.stacktrace IS 'stacktrace';
COMMENT ON COLUMN debug_logger.machine_name IS '서버명';

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

------------------------------
-- 접속로그 View
------------------------------

CREATE OR REPLACE VIEW vw_login_log
AS
	SELECT 
		uuid, 
		user_id, 
		login_ip, 
		user_agent, 
		login_type, 
		user_status, 
		memo, 
		when_expired, 
		watch_url, 
		refresh_token, 
		date_inserted, 
		date_updated
    FROM (
            SELECT 
                ROW_NUMBER() OVER(PARTITION BY user_id ORDER BY date_inserted DESC ) AS row_num
                , *
            FROM com_login_log
         ) LOGIN_STATUS
    WHERE row_num = 1
    ORDER BY user_id;

------------------------------
-- 접속로그 View
------------------------------

CREATE OR REPLACE VIEW vw_user_authrole
AS 
	SELECT 
		user_id,
		auth_order,
		auth_role,
		auth_name,
		memo,
		date_inserted,
		date_updated
    FROM (
            SELECT 
                *
				, (select auth_name from com_authrole where com_authrole.auth_role = com_user_authrole.auth_role LIMIT 1) as auth_name
            FROM com_user_authrole where used_yn = 'Y'
         ) AUTHROLE
    ORDER BY user_id ASC, auth_order ASC, date_inserted ASC;

------------------------------
-- 접속로그 View
------------------------------

CREATE OR REPLACE VIEW vw_user_authrole_primary
AS 
	SELECT 
		user_id,
		auth_role,
		auth_name,
		memo,
		date_inserted,
		date_updated
    FROM (
            SELECT 
                ROW_NUMBER() OVER(PARTITION BY user_id ORDER BY auth_order ASC, date_inserted ASC ) AS row_num
                , *
				, (select auth_name from com_authrole where com_authrole.auth_role = com_user_authrole.auth_role LIMIT 1) as auth_name
            FROM com_user_authrole where used_yn = 'Y'
         ) AUTHROLE
    WHERE row_num = 1
    ORDER BY user_id;

------------------------------
-- 사용자 View
------------------------------
CREATE OR REPLACE VIEW vw_user
AS SELECT 
	com_user.user_id,
	com_user.user_pwd,
	com_user.user_name,
	com_user.nick_name,
	com_user.company_id as company_code,
	(SELECT company_name FROM com_company where com_company.company_id = com_user.company_id LIMIT 1) as company_name,
	com_user.dept_id as dept_code,
    (SELECT dept_name FROM com_dept where com_dept.dept_id = com_user.dept_id LIMIT 1) as dept_name,
	com_user.email,
	com_user.mobile,
	com_user.status,
	com_user.position,
	com_user.location,
	user_authrole.auth_role,
	user_authrole.auth_name,
	com_user.memo AS user_memo, 
	com_user.api_key,
	com_user.when_created as when_created,
	com_user.when_changed as When_Changed,    
	com_user.when_pwd_changed as when_password_changed,
	com_user.when_expired as when_expired,
	com_user.when_synced as when_synced,
	com_user.login_yn AS login_yn,
	com_user.used_yn AS used_yn,
    CASE WHEN com_user.when_expired > now() THEN 'N' ELSE 'Y' END AS expired_yn,
    CASE WHEN 
		   com_user.login_yn = 'N' 
		OR com_user.used_yn = 'N' 
		OR com_user.when_expired <= now() 
	THEN 'N' ELSE 'Y' END AS login_enabled,
	login_log.uuid,
	login_log.user_status,
	login_log.login_type,
	login_log.login_ip,
	login_log.user_agent,
	login_log.date_inserted AS last_logon,
	CASE WHEN login_log.date_inserted = login_log.date_updated THEN null ELSE login_log.date_updated END AS last_logoff,
	login_log.memo AS login_memo
FROM com_user 
LEFT JOIN vw_login_log AS login_log on com_user.user_id = login_log.user_id
LEFT JOIN vw_user_authrole_primary AS user_authrole on com_user.user_id = user_authrole.user_id;

------------------------------
-- OTP View
------------------------------
CREATE OR REPLACE VIEW vw_ota
AS SELECT 
	com_ota.ota_id, 
	com_ota.ota_key,
	com_ota.user_id, 
	t_user.user_name,
	t_user.company_name,
	t_user.dept_name,
	com_ota.client_key, 
	com_ota.when_expired, 
	CASE WHEN com_ota.when_expired > current_timestamp THEN 'N' ELSE 'Y' END AS expired_yn,
	com_ota.used_yn, 
	com_ota.date_inserted,
	com_ota.date_updated
FROM com_ota
LEFT JOIN vw_user AS t_user on t_user.user_id = com_ota.user_id;

/*== End Of Document ========================*/	