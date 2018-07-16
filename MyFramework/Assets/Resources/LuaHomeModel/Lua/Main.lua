module ("LuaHomeModel", package.seeall);

local ModelControl = Class.new(require "LuaHomeModel.HomeModel")

function New(_csobj)
	ModelControl:New(_csobj)
end

function Load(...)
	return ModelControl:Load(...)
end

function LoadEnd()
	return ModelControl:GetEnd()
end

function Start(...)
	return ModelControl:Start(...)
end

function Close()

end
