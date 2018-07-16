local LoginViewComp = Class.define("LoginViewComp",BaseModelViewComp);

function LoginViewComp:ctor(...)
	self:super(LoginViewComp,"ctor",...);
end

function LoginViewComp:Load(...)
	self:super(LoginViewComp,"Load",...);
	self.AccountLoginView = self:Find("AccountLoginView");
	self:AddClick("LoginView01/AccountButton",function (go)
		self.AccountLoginView:Show();
	end)
	self.AccountLoginView:AddClick("LoginButton",function (go)
		self._MyModel:GoToHomeView();
		self:Hide();
	end)
	self:LoadEnd()
end

return LoginViewComp
