@echo 正在生成Thrift文件
@echo off
	for /r %%i in (*.tf) do thrift-0.11.0.exe --gen csharp "%%i"

	::for /r %%i in (*.tf) do thrift-0.9.2.exe --gen csharp:async "%%i"
	::thrift-0.9.2.exe --gen csharp Orders\TOrder.tf
	::thrift-0.9.2.exe --gen csharp Orders\TOrderService.tf
	::请 【不要】 使用管理员方式执行
@echo 生成结束
@pause