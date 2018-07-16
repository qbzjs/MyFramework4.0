local HomeViewComp = Class.define("HomeViewComp",BaseModelViewComp);

function HomeViewComp:ctor(...)
	self:super(HomeViewComp,"ctor",...);
end

function HomeViewComp:Load(...)
	self:super(HomeViewComp,"Load",...);
	self.IsSelf = "liwei1dao"
	self:AddClick("GameButt",function (go)
		ManagerModel:StartLuaModel("LuaDeZhouPKModel",nil,nil);
		self:Hide();
	end)
	self:LoadEnd()
end

return HomeViewComp