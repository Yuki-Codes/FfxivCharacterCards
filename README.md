# FfxivCharacterCards
A .Net utility for generating Final Fantasy XIV character card images

![Demo Card](/Resources/demoCard.png "Demo Card")

Has a simple API surface:
`string path = await FfxivCharacterCards.Card.Generate(characterId, xivApiKey);`
