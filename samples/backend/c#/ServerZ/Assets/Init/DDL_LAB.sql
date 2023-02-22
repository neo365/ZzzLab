/*=============================================
	���� ����
=============================================*/	
--

/*=============================================
	DROP
=============================================*/	
/*-----------------------------------
	View DROP
-----------------------------------*/
DROP VIEW IF EXISTS vw_las_equip;
DROP VIEW IF EXISTS vw_las_status;

/*-----------------------------------
	TABLE DROP
-----------------------------------*/
DROP TABLE IF EXISTS lims_equip_favorites;
DROP TABLE IF EXISTS lims_report;
DROP TABLE IF EXISTS lims_sample;
DROP TABLE IF EXISTS las_result;
DROP TABLE IF EXISTS las_status;
DROP TABLE IF EXISTS las_equip;


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
-- �����
------------------------------
CREATE TABLE las_equip(
	equip_id varchar(36) NOT NULL DEFAULT newid(),
	equip_kind varchar(1000) NOT NULL,
	tag varchar(200) NOT NULL,
	loc varchar(200) NULL,
	avatar varchar(1000),
	settings json,
	memo varchar(8000) NULL,
	api_ver varchar(100) NOT NULL,
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	date_updated timestamp with time zone,
	PRIMARY KEY (equip_id)
);

COMMENT ON TABLE las_equip IS '�������';
COMMENT ON COLUMN las_equip.equip_id IS '�����̵� : �ڵ�����';
COMMENT ON COLUMN las_equip.equip_kind IS '�������';
COMMENT ON COLUMN las_equip.tag IS '���Ī';
COMMENT ON COLUMN las_equip.loc IS '�����ġ';
COMMENT ON COLUMN las_equip.avatar IS '����̹���';
COMMENT ON COLUMN las_equip.settings IS '������';
COMMENT ON COLUMN las_equip.memo IS '�޸�';
COMMENT ON COLUMN las_equip.api_ver IS '����';


INSERT INTO las_equip
(equip_kind, tag, avatar, api_ver)
VALUES
('8130A(-EN) Automated Filter Tester', '8130A(-EN) Automated Filter Tester', 'https://www.abctrd.com/data/product/586afb11940c6.gif', '0.1022.1513.2607'),
('8127/8130(-EN) Automated Filter Tester', '8127/8130(-EN) Automated Filter Tester','https://www.abctrd.com/data/product/558d4a7915cfe.jpg', '0.1022.1513.2607');

------------------------------
-- �׽�Ʈ �α�
------------------------------
CREATE TABLE las_status(
	status_id varchar(36) NOT NULL DEFAULT newid(),
	equip_id varchar(36) NOT NULL REFERENCES las_equip(equip_id),
	user_id varchar(50) REFERENCES com_user(user_id),	
	sample_id varchar(36),
	memo varchar(8000) NULL,
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	date_updated timestamp with time zone,
	PRIMARY KEY (status_id)
);

COMMENT ON TABLE las_status IS '�׽�Ʈ�α�';
COMMENT ON COLUMN las_status.equip_id IS '���';
COMMENT ON COLUMN las_status.user_id IS '�׽���';
COMMENT ON COLUMN las_status.sample_id IS '����';
COMMENT ON COLUMN las_status.memo IS '�޸�';

------------------------------
-- �׽�Ʈ �α�
------------------------------
CREATE TABLE las_result (
	result_id varchar(36) NOT NULL DEFAULT newid(),
	equip_id varchar(36) NOT NULL REFERENCES las_equip(equip_id),
	status_id varchar(36) NOT NULL REFERENCES las_status(status_id),
	sample_id varchar(36),
	result jsonb,
	api_ver varchar(100) NOT NULL,
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	PRIMARY KEY (result_id)
);

COMMENT ON TABLE las_result IS '�׽�Ʈ���';
COMMENT ON COLUMN las_result.api_ver IS '����';

------------------------------
-- ��������
------------------------------
CREATE TABLE lims_sample (
	sample_id varchar(36) NOT NULL DEFAULT newid(),
	sample_name varchar(1000) NOT NULL,
	result jsonb,
	api_ver varchar(100) NOT NULL,
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	date_updated timestamp with time zone,
	PRIMARY KEY (sample_id)
);

------------------------------
-- �׽�Ʈ �α�
------------------------------
CREATE TABLE lims_report (
	report_id varchar(36) NOT NULL DEFAULT newid(),
	equip_kind varchar(1000) NOT NULL,
	result jsonb,
	api_ver varchar(100) NOT NULL,
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	PRIMARY KEY (report_id)
);

------------------------------
-- �׽�Ʈ �α�
------------------------------
CREATE TABLE lims_equip_favorites (
	user_id varchar(1000) NOT NULL,
	equip_id varchar(36) NOT NULL REFERENCES las_equip(equip_id),
	date_inserted timestamp with time zone NOT NULL DEFAULT now(),
	PRIMARY KEY (user_id, equip_id)
);

/*=============================================
	VIEW
=============================================*/	
------------------------------
-- �׽�Ʈ �α�
------------------------------
CREATE OR REPLACE VIEW vw_las_status
AS SELECT *
    FROM (
            SELECT 
                ROW_NUMBER() OVER(PARTITION BY equip_id ORDER BY date_inserted DESC ) AS row_num
                , *
            FROM las_status
         ) LAS_STATUS
    WHERE row_num = 1
    ORDER BY equip_id;

------------------------------
-- �������
------------------------------
CREATE OR REPLACE VIEW vw_las_equip
AS SELECT 
	las_equip.equip_id,
	las_equip.equip_kind as equipment,
	las_equip.tag,
	las_equip.loc,
	las_equip.avatar, 
	las_equip.settings,
	las_equip.memo as memo, 
	las_equip.api_ver AS api_version, 
	las_equip.date_inserted AS when_creasted,
	las_equip.date_updated AS when_updated,
	las_status.status_id, 
	las_status.user_id,
	user_info.user_name,
	user_info.company_name,
	user_info.dept_name,
	las_status.sample_id,
	las_status.memo as status_memo,
	las_status.date_inserted AS when_started,
	las_status.date_updated  AS when_ended	
FROM las_equip 
LEFT JOIN vw_las_status AS las_status on las_equip.equip_id = las_status.equip_id
LEFT JOIN vw_user AS user_info on user_info.user_id = las_status.user_id;

/*== End Of Document ========================*/	