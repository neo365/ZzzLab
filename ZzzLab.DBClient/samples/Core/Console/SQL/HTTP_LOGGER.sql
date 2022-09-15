--[REQUEST]
INSERT INTO debug_http_log (
	  traking_id
	, user_id
	, ip_addr
	, user_agent
	, method
	, path
	, query_string
	, protocol
	, request_header
	, request_body
	, date_request
	, execute_time
	, machine_name
) VALUES (
	  @traking_id
	, @user_id
	, @ip_addr
	, @user_agent
	, @method
	, @path
	, @query_string
	, @protocol
	, @request_header
	, @request_body
	, @date_request
	, @execute_time
	, @machine_name
)

--[RESPONSE]
UPDATE debug_http_log SET
	status_code = @status_code,
	response_header = @response_header,
	response_body = @response_body,
	date_response = @date_response,
	execute_time = @execute_time
WHERE traking_id = @traking_id;

--[MERGE]
MERGE INTO debug_http_log AS REQUEST
USING (
	SELECT
		  @traking_id		AS traking_id
		, @status_code		AS status_code
		, @response_header	AS response_header
		, @response_body	AS response_body
		, @date_response	AS date_response
		, @execute_time		AS execute_time
		, @machine_name		AS machine_name
) AS RESPONSE
ON (REQUEST.traking_id = RESPONSE.traking_id)
WHEN MATCHED THEN
	UPDATE SET
		status_code = @status_code,
		response_header = response_header,
		response_body = response_body,
		date_response = date_response,
		execute_time = execute_time
WHEN NOT MATCHED THEN
	INSERT (
		  traking_id
		, status_code
		, response_header
		, response_body
		, date_response
		, execute_time
		, machine_name
	) VALUES (
		  @traking_id
		, @status_code
		, @response_header
		, @response_body
		, @date_response
		, @execute_time
		, @machine_name
	)
;