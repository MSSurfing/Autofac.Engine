namespace * MSSurfing.TClient.Processors.Users
include "../../Domain/Users/TUser.tf"

/** 用户处理器 */
service TUserProcessor
{
	/** 用户
	* @return Dictionary<int,list<TUser.TUser>> <int:Code(1=Success,负数为不成功) ,string:成功时返回的Data>
	*/
	map<i32,list<TUser.TUser>> Search(5:string name)
}