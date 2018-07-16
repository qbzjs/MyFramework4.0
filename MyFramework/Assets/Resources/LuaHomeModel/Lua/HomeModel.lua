local HomeModel = Class.define("HomeModel",BaseModel)

function HomeModel:New(_csobj)
	self:super(HomeModel,"New", _csobj);
end

function HomeModel:Load(...)
	self.CSModelObj:LoadResourceComp()
	self:AddComp("LoginViewComp",require "LuaHomeModel.LoginViewComp","LoginView")
	self:super(HomeModel,"Load", ...);
end


function HomeModel:GoToHomeView()
	self:AddComp("HomeViewComp",require "LuaHomeModel.HomeViewComp","HomeView")
end


return HomeModel