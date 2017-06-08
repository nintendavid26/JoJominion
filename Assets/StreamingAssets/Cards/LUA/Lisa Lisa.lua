--Lisa Lisa
function Use()
	Card.SelectCard(User);
end

Selectable={};
function SelectCards()
Selectable=User.Hand;
return Selectable
end


function OnSelect()
	x=0
	if Selected== nil then x=0 
	else User.Discard(Selected,User.Hand);User.GetMoney(6);
	end
end