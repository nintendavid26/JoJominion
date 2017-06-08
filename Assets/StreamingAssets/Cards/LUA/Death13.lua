--Death13
function Use()
	Card.SelectCard(User);
end

Selectable={};
function SelectCards()
	Selectable=User.Hand;
return Selectable;
end

function OnSelect()
User.GetMoney(Selected.Cost+1);
User.Destroy(Selected,User.Hand);
return Selected;
end
