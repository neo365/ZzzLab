--[GET]
SELECT
	*
FROM debug_logger
WHERE log_id = @log_id

--[LIST]
SELECT
	*
FROM debug_logger
WHERE logger = @logger
--{search}
--{orderby}
;

--[LIST_RECORDS_TOTAL]
SELECT COUNT(*) FROM debug_logger WHERE logger = @logger

--[LIST_RECORDS_FILTERED]
SELECT COUNT(*) FROM debug_logger
WHERE logger = @logger
--{search}

--[CLEAN]
DELETE
FROM debug_logger
WHERE logger = @logger

--[CLEAN_ALL]
DELETE
FROM debug_logger
WHERE logger = @logger

--[DELETE_ITEM]
DELETE
FROM debug_logger
WHERE	logger = @logger
	AND log_id = @log_id
