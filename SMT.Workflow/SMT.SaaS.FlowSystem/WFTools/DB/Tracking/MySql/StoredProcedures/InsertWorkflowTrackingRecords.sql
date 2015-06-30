DELIMITER $$

DROP PROCEDURE IF EXISTS InsertWorkflowTrackingRecords $$
/*
 * Insert a batch of workflow tracking records.
 */	
CREATE PROCEDURE InsertWorkflowTrackingRecords
(
	IN p_WORKFLOW_INSTANCE_ID BIGINT UNSIGNED
	,IN p_WORKFLOW_INSTANCE_STATUS_1 SMALLINT UNSIGNED
	,IN p_EVENT_DATE_TIME_1 DATETIME
	,IN p_EVENT_ORDER_1 INT UNSIGNED
	,IN p_EVENT_ARG_TYPE_NAME_1 VARCHAR(128)
	,IN p_EVENT_ARG_ASSEMBLY_NAME_1 VARCHAR(256)
	,IN p_EVENT_ARG_1 BLOB
	,OUT p_WORKFLOW_INSTANCE_EVENT_ID_1 BIGINT UNSIGNED
	,IN p_WORKFLOW_INSTANCE_STATUS_2 SMALLINT UNSIGNED
	,IN p_EVENT_DATE_TIME_2 DATETIME
	,IN p_EVENT_ORDER_2 INT UNSIGNED
	,IN p_EVENT_ARG_TYPE_NAME_2 VARCHAR(128)
	,IN p_EVENT_ARG_ASSEMBLY_NAME_2 VARCHAR(256)
	,IN p_EVENT_ARG_2 BLOB
	,OUT p_WORKFLOW_INSTANCE_EVENT_ID_2 BIGINT UNSIGNED
	,IN p_WORKFLOW_INSTANCE_STATUS_3 SMALLINT UNSIGNED
	,IN p_EVENT_DATE_TIME_3 DATETIME
	,IN p_EVENT_ORDER_3 INT UNSIGNED
	,IN p_EVENT_ARG_TYPE_NAME_3 VARCHAR(128)
	,IN p_EVENT_ARG_ASSEMBLY_NAME_3 VARCHAR(256)
	,IN p_EVENT_ARG_3 BLOB
	,OUT p_WORKFLOW_INSTANCE_EVENT_ID_3 BIGINT UNSIGNED
	,IN p_WORKFLOW_INSTANCE_STATUS_4 SMALLINT UNSIGNED
	,IN p_EVENT_DATE_TIME_4 DATETIME
	,IN p_EVENT_ORDER_4 INT UNSIGNED
	,IN p_EVENT_ARG_TYPE_NAME_4 VARCHAR(128)
	,IN p_EVENT_ARG_ASSEMBLY_NAME_4 VARCHAR(256)
	,IN p_EVENT_ARG_4 BLOB
	,OUT p_WORKFLOW_INSTANCE_EVENT_ID_4 BIGINT UNSIGNED
	,IN p_WORKFLOW_INSTANCE_STATUS_5 SMALLINT UNSIGNED
	,IN p_EVENT_DATE_TIME_5 DATETIME
	,IN p_EVENT_ORDER_5 INT UNSIGNED
	,IN p_EVENT_ARG_TYPE_NAME_5 VARCHAR(128)
	,IN p_EVENT_ARG_ASSEMBLY_NAME_5 VARCHAR(256)
	,IN p_EVENT_ARG_5 BLOB
	,OUT p_WORKFLOW_INSTANCE_EVENT_ID_5 BIGINT UNSIGNED
)
sproc:BEGIN
	-- parameter set 1
	CALL InsertWorkflowTrackingRecord(p_WORKFLOW_INSTANCE_ID, 
		p_WORKFLOW_INSTANCE_STATUS_1, p_EVENT_DATE_TIME_1, 
		p_EVENT_ORDER_1, p_EVENT_ARG_TYPE_NAME_1, 
		p_EVENT_ARG_ASSEMBLY_NAME_1, p_EVENT_ARG_1, 
		p_WORKFLOW_INSTANCE_EVENT_ID_1);
		
	-- parameter set 2
	IF p_WORKFLOW_INSTANCE_STATUS_2 IS NULL THEN
		LEAVE sproc;
	END IF;

	CALL InsertWorkflowTrackingRecord(p_WORKFLOW_INSTANCE_ID, 
		p_WORKFLOW_INSTANCE_STATUS_2, p_EVENT_DATE_TIME_2, 
		p_EVENT_ORDER_2, p_EVENT_ARG_TYPE_NAME_2, 
		p_EVENT_ARG_ASSEMBLY_NAME_2, p_EVENT_ARG_2, 
		p_WORKFLOW_INSTANCE_EVENT_ID_2);

	-- parameter set 3
	IF p_WORKFLOW_INSTANCE_STATUS_3 IS NULL THEN
		LEAVE sproc;
	END IF;

	CALL InsertWorkflowTrackingRecord(p_WORKFLOW_INSTANCE_ID, 
		p_WORKFLOW_INSTANCE_STATUS_3, p_EVENT_DATE_TIME_3, 
		p_EVENT_ORDER_3, p_EVENT_ARG_TYPE_NAME_3, 
		p_EVENT_ARG_ASSEMBLY_NAME_3, p_EVENT_ARG_3, 
		p_WORKFLOW_INSTANCE_EVENT_ID_3);
				
	-- parameter set 4
	IF p_WORKFLOW_INSTANCE_STATUS_4 IS NULL THEN
		LEAVE sproc;
	END IF;

	CALL InsertWorkflowTrackingRecord(p_WORKFLOW_INSTANCE_ID, 
		p_WORKFLOW_INSTANCE_STATUS_4, p_EVENT_DATE_TIME_4, 
		p_EVENT_ORDER_4, p_EVENT_ARG_TYPE_NAME_4, 
		p_EVENT_ARG_ASSEMBLY_NAME_4, p_EVENT_ARG_4, 
		p_WORKFLOW_INSTANCE_EVENT_ID_4);
		
	-- parameter set 5
	IF p_WORKFLOW_INSTANCE_STATUS_5 IS NULL THEN
		LEAVE sproc;
	END IF;

	CALL InsertWorkflowTrackingRecord(p_WORKFLOW_INSTANCE_ID, 
		p_WORKFLOW_INSTANCE_STATUS_5, p_EVENT_DATE_TIME_5, 
		p_EVENT_ORDER_5, p_EVENT_ARG_TYPE_NAME_5, 
		p_EVENT_ARG_ASSEMBLY_NAME_5, p_EVENT_ARG_5, 
		p_WORKFLOW_INSTANCE_EVENT_ID_5);

END $$

DELIMITER ;
