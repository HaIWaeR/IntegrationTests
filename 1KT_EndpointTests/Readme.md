# GET Получение книг
### Script
```js
    // Статус 200 OK
    pm.test("Status code is 200", function () {
        pm.response.to.have.status(200);
    });

    // Это json
    pm.test("Responce is valid JSON", function() {
        pm.response.to.have.json;
    });

    // Это массив
    pm.test("Response is an array", function () {
        const jsonData = pm.response.json();
        pm.expect(jsonData).an("array");
    });

    // Масив не пустой
    pm.test("Array is not empty", function () {
        const jsonData = pm.response.json();
        pm.expect(jsonData).lengthOf.above(0);
    });

    // Имеет все нужные поля
    pm.test("Each book has required fields", function () {
        const jsonData = pm.response.json();

        jsonData.forEach(function (book) {
            pm.expect(book).property("id");
            pm.expect(book).property("title");
            pm.expect(book).property("author");
            pm.expect(book).property("year");
            pm.expect(book).property("genre");
            pm.expect(book).property("price");
            pm.expect(book).property("isAvailable");
        });
    });

    // Правильный тип данных у всех полей
    pm.test("Field types are correct", function(){
        const jsonData = pm.response.json();

        jsonData.forEach(function (book, index) {
            pm.expect(book.id, `book at index ${index}`).a("number");
            pm.expect(book.title, `book at index ${index}`).a("string");
            pm.expect(book.author, `book at index ${index}`).a("string");
            pm.expect(book.year, `book at index ${index}`).a("number");
            pm.expect(book.genre, `book at index ${index}`).a("string");
            pm.expect(book.price, `book at index ${index}`).a("number");
            pm.expect(book.isAvailable, `book at index ${index}`).a("boolean");
        });

    });

    // Проверка, что тело ответа содержит правильный ответ.
    pm.test("Check the body contain the correct answer", function () {
        var jsonData = pm.response.json();
        pm.expect(jsonData[0].author).to.eql("George Orwell");
    });

    // Замер скорости ответа на запрос.
    pm.test("Responce time is less than 500ms", function (){
        pm.expect(pm.response.responseTime).to.be.below(100);
    });
```

Фото[]

# POST Создание книг
### Body
```js
{
  "title": "Зелёная лампа",
  "author": "Грин",
  "year": 1890,
  "genre": "Рассказ",
  "price": 500,
  "isAvailable": true
}
```
### Script
```js
// Статус 201 
pm.test("Status code is 201", function (){
    pm.response.to.have.status(201);
});

// в ответе есть сгенерированный id
pm.test("Response contains generated id", function () {
    const jsonData = pm.response.json();

    pm.expect(jsonData).property("id");
    pm.expect(jsonData.id).a("number");
    pm.expect(jsonData.id).to.be.above(0);
});

// Поля ответа совпадают с тем, что отправили 
pm.test("Created book matches sent data", function () {
    const sent = JSON.parse(pm.request.body.raw);
    const received = pm.response.json();

    pm.expect(received.title).eql(sent.title);
    pm.expect(received.author).eql(sent.author);
    pm.expect(received.year).eql(sent.year);
    pm.expect(received.genre).eql(sent.genre);
    pm.expect(received.price).eql(sent.price);
    pm.expect(received.isAvailable).eql(sent.isAvailable);
});

// сохранить id в переменную для следующих запросов 
const created = pm.response.json();
pm.collectionVariables.set("bookId", created.id);
pm.collectionVariables.set("bookTitle", created.title);
pm.collectionVariables.set("bookAuthor", created.author);
```

# GET by {Id} Получение по Id 
### Script
```js
// Статус 200 Ok
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// Это объект 
pm.test("Response is a single object", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData).a("object");
    pm.expect(jsonData).not.a("array");
});

// совпадение id с запросом
pm.test("Returned id matches requested id", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData.id).to.eql(Number(pm.collectionVariables.get("bookId")));
});

// значение полей соответствуют ожидаемым
pm.test("Book data is correct", function () {
    const jsonData = pm.response.json();

    pm.expect(jsonData.title).to.eql(pm.collectionVariables.get("bookTitle"));
    pm.expect(jsonData.author).to.eql(pm.collectionVariables.get("bookAuthor"));
});
```
# GET by {Id} Нигативные тесты по id
### Script
```js
pm.test("Status code is 404 for non-existent id", function() {
    pm.response.to.have.status(404);
});

pm.test("Error message is present", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData).property("message");
});
```
# PATCH Изменения книги
### Body
```js
{
    "price": 700,
    "isAvailable": false
}
```
### Script
```js
// Статус 200
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// Поля, которые были изменены, изменились.
pm.test("Patched fields were changed", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData.price).to.eql(700);
    pm.expect(jsonData.isAvailable).to.be.false;
});

// Поля которые не должны измениться, должны остаться неизменными. 
pm.test("Other fields stayed untouched", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData.title).to.eql(pm.collectionVariables.get("bookTitle"));
    pm.expect(jsonData.author).to.eql(pm.collectionVariables.get("bookAuthor"));
});
```
# PUT Обновление книги
### Body 
```js
{
  "title": "Собака баскервилей",
  "author": "Конан дойл",
  "year": 2000,
  "genre": "Ужастик",
  "price": 1000,
  "isAvailable": true
}
```
### Script
```js
// статус 200 ok
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
}); 

// изменённые поля дейсвиетльно изменились 
pm.test("Fields were updated", function () {
    const jsonData = pm.response.json();

    pm.expect(jsonData.genre).to.eql("Ужастик");
    pm.expect(jsonData.price).to.eql(1000);
    pm.expect(jsonData.isAvailable).to.be.true;
});

// Проверка что поля изменились а не создались
pm.test("Id did not change", function () {
    pm.expect(pm.response.json().id).to.eql(Number(pm.collectionVariables.get("bookId")));
}); 
```
# DELETE Удаление книги
### Script
```js
// Статус 200 или 204
pm.test("Status code is 200 or 204", function () {
    pm.expect(pm.response.code).to.be.oneOf([200, 204]);
});

// Тело ответа пустое для статуса 204
pm.test("Response body is empty", function () {
    pm.expect(pm.response.text()).to.be.empty;
});

// Проверка, что удаленная книга больше не найдена
pm.test("Deleted book is not found anymore", function (done) {
    const url = pm.variables.replaceIn("{{url}}/api/books/{{bookId}}");

    pm.sendRequest(url, function (err, res) {
        if (err) { return done(err); }
        pm.expect(res.code).to.eql(404);
        done();
    });
});
```