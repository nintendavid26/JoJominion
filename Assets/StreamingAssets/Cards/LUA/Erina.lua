--Erin
function Use()
	User.GetActions(1)
	x=1
	for i=0,User.InPlay.Count-1,1 do
	if User.InPlay[i].Cost>7 then x=4 end
	end
	User.GetMoney(x);
end
