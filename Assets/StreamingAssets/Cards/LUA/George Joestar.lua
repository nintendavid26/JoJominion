--George Joestar
function Use()
	
	Controller.Wait(Card.Name);
	
end

Selectable={};
function SelectCards()
x=0
for i=0,9,1 do
	if Shop[i].Cost<=5 then Selectable[x]=Shop[i];x=x+1; end
	end
return Selectable;
end

function Use2()
User.Gain(Selected.Name,User.Discarded,true);
return Selected;
end
