--[GET]
SELECT
	*
FROM vw_ota
WHERE ota_id = @ota_id

--[LIST]
SELECT
	*
FROM vw_ota

--[INSERTED]
INSERT INTO com_ota(
	ota_key,
	user_id,
	client_key
) VALUES (
	@ota_key,
	@user_id,
	@client_key
);

--[AUTH]
UPDATE com_ota SET
when_used = now(), 
date_updated = now()
WHERE ota_key = @ota_key 
	AND date_updated is null 
	AND when_expired > now()
	AND used_yn = 'Y'
RETURNING user_id;


