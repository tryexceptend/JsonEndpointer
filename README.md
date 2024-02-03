# JsonEndpointer
**JsonEndpointer** - обертка над JsonDocument, которая позволяет получить из json текста значение какого то конкретного ключа по пути к нему.

Например есть такой json документ:

```json
{[
 {
 "following": false,
 "id": "1393174363",
 "screen_name": "Project",
 "name": "Test Project",
 "protected": false,
 "count": 32289,
 "formatted_followers_count": "32.3K followers",
 "age_gated": false
 }
]}
```

Вызов выведет: Project

```
using JsonDocument doc = JsonDocument.Parse(json);
JsonElement root = doc.RootElement;
Console.WriteLine(root.GetKeyToString("[0].name"));
```
