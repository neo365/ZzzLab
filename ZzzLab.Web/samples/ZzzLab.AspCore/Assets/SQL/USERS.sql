--[GET]
SELECT
	*
FROM vw_user
WHERE user_id = @user_id

--[LIST]
SELECT
	*
FROM vw_user
WHERE 1 = 1
--{search}
--{orderby}
;

--[LIST_RECORDS_TOTAL]
SELECT COUNT(*) FROM vw_user;

--[LIST_RECORDS_FILTERED]
SELECT COUNT(*) FROM vw_user
WHERE 1 = 1
--{search}

--[INSERT]
INSERT INTO com_user(
	user_id,
	user_name,
	nick_name,
	company_id,
	dept_id,
	email,
	mobile,
	auth_role,
	login_yn,
	used_yn,
	memo,
	when_created,
	when_expired,
	api_key
) VALUES (
	@user_id,
	@user_name,
	@nick_name,
	@company_id,
	@dept_id,
	@email,
	@mobile,
	@auth_role,
	@login_yn,
	@used_yn,
	@memo,
	now(),
	CAST (@when_expired AS DATE),
	newid()
);

--[UPDATE]
UPDATE com_user SET
	user_name = @user_name,
	nick_name = @nick_name,
	company_id = @company_id,
	dept_id = @dept_id,
	email = @email,
	mobile = @mobile,
	auth_role = @auth_role,
	login_yn = @login_yn,
	when_expired = CAST (@when_expired AS DATE),
	used_yn = @used_yn,
	memo = @memo,
	when_changed = now() 
WHERE user_id = @user_id;

--[UPDATE_USER]
UPDATE com_user SET
	nick_name = @nick_name,
	email = @email,
	mobile = @mobile,
	when_changed = now() 
WHERE user_id = @user_id;

--[DELETE]
UPDATE com_user SET
	used_yn = 'N'
WHERE	user_id = @user_id
	AND used_yn = 'Y'

--[DELETE_TRUNCATE]
DELETE
FROM com_user
WHERE user_id = @user_id
