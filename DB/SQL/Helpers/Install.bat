@echo off
rem Installs all scripting helpers
setlocal enabledelayedexpansion
set SQLCMDSERVER=.\SQLEXPRESS
set SQLCMDDBNAME=SPCA
set SQLCMDUSER=act
set SQLCMDPASSWORD=act1user

sqlcmd -i sequence.sql,drop_if_exists.sql,print_long.sql,print_def.sql,find_def.sql,str_split.sql,col_def.sql,describe_object.sql,df_create.sql,fk_create.sql,index_id.sql,ix_create.sql,tbl_def.sql,keep_chars.sql