--[ROOT]
SELECT
	'company' AS code_type,
	company_id as id,
	company_name as name,
	case 
		WHEN (select count(*) from com_company where parent_id = company.company_id) > 0 THEN 'Y'
		WHEN (select count(*) from com_dept where company_id = company.company_id) > 0 THEN 'Y'
		ELSE 'N' 
	END AS HAS_CHILD
FROM com_company company
WHERE use_yn = 'Y'
AND parent_id is null
;

--[CHILDREN]
SELECT
	'company' AS code_type,
	company_id as id,
	company_name as name,
	case 
		WHEN (select count(*) from com_company where parent_id = company.company_id) > 0 THEN 'Y'
		WHEN (select count(*) from com_dept where company_id = company.company_id) > 0 THEN 'Y'
		ELSE 'N' 
	END AS HAS_CHILD
FROM com_company company
WHERE use_yn = 'Y'
AND parent_id = @current_id
UNION
SELECT
	'dept' AS code_type,
	dept_id as id,
	dept_name as name,
	case
		WHEN (select count(*) from com_dept where parent_id = dept.dept_id) > 0 THEN 'Y'
		WHEN (select count(*) from com_user where dept_id = dept.dept_id) > 0 THEN 'Y'
		ELSE 'N' 
	END AS HAS_CHILD
FROM com_dept dept
WHERE use_yn = 'Y'
AND company_id = @current_id
UNION
SELECT
	'user' AS code_type,
	user_id as id,
	user_name as name,
	'N' AS HAS_CHILD
FROM com_user
WHERE use_yn = 'Y'
AND dept_id = @current_id