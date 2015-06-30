DROP TABLE IF EXISTS ADDED_ACTIVITY;
CREATE TABLE ADDED_ACTIVITY
(
	WORKFLOW_INSTANCE_ID BIGINT UNSIGNED NOT NULL
	,WORKFLOW_INSTANCE_EVENT_ID BIGINT UNSIGNED NOT NULL
	,QUALIFIED_NAME VARCHAR(128) NOT NULL
	,ACTIVITY_TYPE_ID BIGINT UNSIGNED NOT NULL
	,PARENT_QUALIFIED_NAME VARCHAR(128) NULL
	,ADDED_ACTIVITY_ACTION VARCHAR(2000) NULL
	,`ORDER` INT UNSIGNED NULL
);

CREATE INDEX ADDED_ACTIVITY_IDX01 ON ADDED_ACTIVITY ( WORKFLOW_INSTANCE_ID );
CREATE INDEX ADDED_ACTIVITY_IDX02 ON ADDED_ACTIVITY ( WORKFLOW_INSTANCE_EVENT_ID );
CREATE INDEX ADDED_ACTIVITY_IDX03 ON ADDED_ACTIVITY ( QUALIFIED_NAME );