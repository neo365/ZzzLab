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
DROP VIEW IF EXISTS vw_user_log;

/*-----------------------------------
	Table DROP
-----------------------------------*/
/* Debug */
DROP TABLE IF EXISTS debug_http_log;
DROP TABLE IF EXISTS debug_logger;

/* Auth */
DROP TABLE IF EXISTS com_user_auth;
DROP TABLE IF EXISTS com_auth;

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

/*=============================================
	TABLE
=============================================*/	
------------------------------
-- 회사정보
------------------------------

CREATE TABLE com_company (
	company_id varchar(36) NOT NULL DEFAULT newid(),
	company_name varchar(200),
    parent_id varchar(36),
    use_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,    
	PRIMARY KEY (company_id)
);

COMMENT ON TABLE com_company IS '회사정보';

/* Default Value */
INSERT INTO com_company ( company_id, company_name, parent_id, use_yn, memo) 
VALUES 
  ('00000000-0000-0000-0000-000000000000',  'ZzzLab', null, 'Y', null)
;
------------------------------
-- 부서정보
------------------------------

CREATE TABLE com_dept (
	dept_id varchar(36) NOT NULL DEFAULT newid(),
	dept_name varchar(200),
    company_id varchar(36) NOT NULL REFERENCES com_company(company_id),
    parent_id varchar(36),
    use_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,    
	PRIMARY KEY (dept_id)
);	

COMMENT ON TABLE com_dept IS '부서정보';

/* Default Value */
INSERT INTO com_dept ( company_id, dept_id, dept_name, parent_id, use_yn, memo) 
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
	company_id varchar(36) REFERENCES com_company(company_id),
	dept_id varchar(36) REFERENCES com_dept(dept_id),
	email varchar(200),
	mobile varchar(50),
	auth_role varchar(100),
	login_yn char(1) NOT NULL DEFAULT 'Y',
	use_yn char(1) NOT NULL DEFAULT 'Y',
	memo varchar(8000),
	when_created timestamp with time zone NOT NULL DEFAULT now(),
	when_changed timestamp with time zone,
	when_pwd_changed date DEFAULT now(),
	when_expired date NOT NULL DEFAULT '9999-12-31',
	api_key varchar(36) NOT NULL DEFAULT newid(),
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
COMMENT ON COLUMN com_user.auth_role IS '기본권한';
COMMENT ON COLUMN com_user.login_yn IS '로그인가능여부 (Y/N)';
COMMENT ON COLUMN com_user.use_yn IS '사용여부 (Y/N)';
COMMENT ON COLUMN com_user.memo IS '메모';
COMMENT ON COLUMN com_user.when_created IS '생성일';
COMMENT ON COLUMN com_user.when_changed IS '변경일';
COMMENT ON COLUMN com_user.when_pwd_changed IS '패스워드 변경일';
COMMENT ON COLUMN com_user.when_expired IS '사용만료일';
COMMENT ON COLUMN com_user.api_key IS '사용자 고유 접근 코드';

CREATE INDEX idx_com_user_name ON com_user USING btree (user_name ASC) TABLESPACE pg_default;
	
/* Default Value */
INSERT INTO com_user ( user_id, user_pwd, user_name, company_id, dept_id, auth_role, login_yn, memo) 
VALUES 
  ('admin',  null, '관리자', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'ADMIN_ROLE',  'N', '시스템운영계정')
, ('devel',  null, '개발자', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'SYSTEM_ROLE', 'N', '개발자계정')
, ('SYSTEM', null, '시스템', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'SYSTEM_ROLE', 'N', '시스템운영계정. 로그인안됨')
, ('neo365', null, '김영동', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'USER_ROLE', 'Y', null)
, ('soundmax', null, '쏨위드', '00000000-0000-0000-0000-000000000000', '00000000-0000-0000-0000-000000000000', 'USER_ROLE', 'Y', null)
;

------------------------------
-- OTP / SSO
------------------------------
CREATE TABLE com_ota (
	ota_id varchar(36) NOT NULL DEFAULT newid(),
	ota_key varchar(1000) NOT NULL,
	user_id varchar(50) NOT NULL REFERENCES com_user(user_id),
	client_key varchar(36) NOT NULL,
	login_ip varchar(15) NOT NULL,	
	user_agent varchar(1000) NOT NULL,
	when_expired timestamp with time zone NOT NULL DEFAULT current_timestamp + (5 * interval '1 minute'),
	used_yn char(1) NOT NULL DEFAULT 'Y',
	memo varchar(8000),
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone, 
	PRIMARY KEY (ota_id)
);

------------------------------
-- 권한정보
------------------------------

CREATE TABLE com_auth (
	auth_role varchar(50) NOT NULL,
	auth_name varchar(100),
	auth_desc varchar(8000),
    auth_rule jsonb,
    use_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,  	
	PRIMARY KEY (auth_role)
);

COMMENT ON TABLE com_auth IS '권한정보';

INSERT INTO com_auth ( auth_role, auth_name, auth_desc, auth_rule) 
VALUES 
  ('ADMIN_ROLE',  '관리자', null, null)
, ('SYSTEM_ROLE', '시스템', null, null)
, ('USER_ROLE',   '사용자', null, null)
;

------------------------------
-- 사용자 권한정보
------------------------------

CREATE TABLE com_user_auth (
    user_id varchar(50) NOT NULL  REFERENCES com_user(user_id),
	auth_role varchar(50) NOT NULL  REFERENCES com_auth(auth_role),
    auth_order int NOT NULL DEFAULT '0',
    use_yn char(1) NOT NULL DEFAULT 'Y',
    memo varchar(8000),
    date_inserted timestamp with time zone NOT NULL DEFAULT now(),
    date_updated timestamp with time zone,  	
	PRIMARY KEY (user_id, auth_role)
);

COMMENT ON TABLE com_user_auth IS '사용자 권한정보';

INSERT INTO com_user_auth ( user_id, auth_role) 
VALUES 
  ('admin', 'ADMIN_ROLE')
, ('devel', 'SYSTEM_ROLE')
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
COMMENT ON COLUMN com_login_log.date_inserted IS '접속일시';
COMMENT ON COLUMN com_login_log.date_updated IS '갱신일시';

CREATE INDEX idx_sso_dt ON com_login_log USING btree (date_inserted DESC NULLS FIRST);

/*=============================================
	VIEW
=============================================*/	

------------------------------
-- 접속로그 View
------------------------------

CREATE OR REPLACE VIEW vw_user_log
AS SELECT *
    FROM (
            SELECT 
                ROW_NUMBER() OVER(PARTITION BY user_id ORDER BY date_inserted DESC ) AS row_num
                , *
            FROM com_login_log
         ) SSO_STATUS
    WHERE row_num = 1
    ORDER BY user_id;

------------------------------
-- 사용자 View
------------------------------
CREATE OR REPLACE VIEW vw_user
AS SELECT 
	com_user.user_id,
	user_log.uuid,
	user_log.user_status,
	user_log.login_type,
	user_log.login_ip,
	user_log.user_agent,
	com_user.user_pwd,
	com_user.user_name,
	com_user.nick_name,
	com_user.company_id,
	(SELECT company_name FROM com_company where com_company.company_id = com_user.company_id LIMIT 1) as company_name,
	com_user.dept_id,
    (SELECT dept_name FROM com_dept where com_dept.dept_id = com_user.dept_id LIMIT 1) as dept_name,
	com_user.email,
	com_user.mobile,
	com_user.auth_role, 
	com_user.memo AS user_memo, 
	user_log.memo AS login_memo,
	com_user.when_Created as When_Created,
	com_user.when_Changed as When_Changed,    
	com_user.when_pwd_changed as Pwd_LastSet,
	com_user.when_expired as When_Expired,
	user_log.date_inserted AS LastLogOn,
	CASE WHEN user_log.date_inserted = user_log.date_updated THEN null ELSE user_log.date_updated END AS LastLogOff,
	com_user.login_yn AS Is_Login,
	com_user.use_yn AS Is_Enable,
    CASE WHEN com_user.when_expired > now() THEN 'N' ELSE 'Y' END AS Is_Expired,
    CASE WHEN 
		   com_user.login_yn = 'N' 
		OR com_user.use_yn = 'N' 
		OR com_user.when_expired <= now() 
	THEN 'N' ELSE 'Y' END AS Is_LoginEnable,
	com_user.memo
FROM com_user 
LEFT JOIn vw_user_log AS user_log on com_user.user_id = user_log.user_id;

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
	com_ota.login_ip, 
	com_ota.user_agent, 
	com_ota.when_expired, 
	CASE WHEN com_ota.when_expired > current_timestamp THEN 'N' ELSE 'Y' END AS expired_yn,
	com_ota.used_yn, 
	com_ota.date_inserted,
	com_ota.date_updated
FROM com_ota
LEFT JOIN vw_user AS t_user on t_user.user_id = com_ota.user_id;

/*== End Of Document ========================*/	