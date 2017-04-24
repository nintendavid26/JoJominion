--Hey Ya
function Use()
	Controller.Wait();
end

Selectable={};
function SelectCards()
Selectable=User.RevealTop(2);
return Selectable
end


function Use2()
	for i=0,1,1 do
		if User.Deck[0] == Selected then User.AddToHand(User.Deck[0],User.Deck);
		else User.Discard(User.Deck[0],User.Deck);
		end
	end
end