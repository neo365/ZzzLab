--[GET_LOGIN]
SELECT 
	user_id, 
	user_pwd,
	com_user.used_yn AS used_yn,
	CASE WHEN com_user.when_expired > now() THEN 'N' ELSE 'Y' END AS expired_yn,
    CASE WHEN 
		   com_user.login_yn = 'N' 
		OR com_user.used_yn = 'N' 
		OR com_user.when_expired <= now() 
	THEN 'N' ELSE 'Y' END AS login_enabled
FROM com_user
WHERE user_id = @user_id

--[UPDATE_PASSWORD]
UPDATE com_user SET
	user_pwd = @user_pwd
WHERE user_id = @user_id

--[SINGCHECK]
SELECT 
	login_ip, 
	user_agent
FROM com_login_log
WHERE	1 = 1
	AND uuid = @uuid
	AND user_id = @user_id
	AND login_ip = @login_ip

--[SIGNIN]
INSERT INTO com_login_log(
	uuid,
	user_id, 
	login_ip,
	user_agent, 
	login_type,
	user_status,
	memo, 
	refresh_token,
	date_inserted
) 
SELECT
	  @uuid
	, user_id
	, @login_ip
	, @user_agent
	, @login_type
	, 'Open'
	, @memo
	, @refresh_token
	, now()
FROM com_user
WHERE user_id = @user_id
LIMIT 1

--[SIGNOUT]
UPDATE com_login_log SET
	user_status = 'Close',
	memo = @memo,
	date_updated = now()
WHERE uuid = @uuid;


--[IS_EXPIRED]
SELECT 
	CASE WHEN when_expired >= current_timestamp THEN 'N' ELSE 'Y' END AS is_expired 
FROM com_login_log
WHERE uuid = @uuid ;

--[SESSION_EXPIRED]
UPDATE com_login_log SET
	when_expired = current_timestamp + (20 * interval '1 minute'),
	watch_url = @watch_url,
	date_updated = now()
WHERE uuid = @uuid;

--[GET_FROM_REFRESH_TOKEN]
SELECT 
*
FROM com_login_log
WHERE refresh_token = @refresh_token;
