# 10. Описание API и контрактов

WPF-клиент взаимодействует с серверной частью через HTTP REST API.

Ниже приведены предлагаемые основные точки входа.

| Метод | Адрес | Назначение |
|---|---|---|
| `GET` | `/api/v1/books` | Получение и поиск книг |
| `POST` | `/api/v1/books` | Добавление книги |
| `GET` | `/api/v1/books/{id}` | Получение карточки книги |
| `GET` | `/api/v1/books/{id}/copies` | Получение экземпляров книги |
| `POST` | `/api/v1/copies` | Добавление экземпляра |
| `GET` | `/api/v1/copies/{id}` | Получение сведений об экземпляре |
| `GET` | `/api/v1/readers` | Получение списка читателей |
| `POST` | `/api/v1/readers` | Регистрация читателя |
| `GET` | `/api/v1/readers/{id}` | Получение карточки читателя |
| `POST` | `/api/v1/loans` | Оформление выдачи |
| `POST` | `/api/v1/loans/{id}/return` | Регистрация возврата |
| `GET` | `/api/v1/loans` | Получение списка выдач |
| `GET` | `/api/v1/reports/fund` | Отчет о библиотечном фонде |
| `GET` | `/api/v1/reports/overdue` | Отчет о просроченных возвратах |

## 10.1. Контракт BookDto

Представляет библиографическую запись.

```text
BookDto
- id: integer
- title: string
- isbn: string | null
- publisher: string | null
- publication_year: integer | null
- authors: string[]
```

## 10.2. Контракт CopyDto

Представляет отдельный экземпляр книги.

```text
CopyDto
- id: integer
- book_id: integer
- inventory_number: string
- status: string
```

## 10.3. Контракт ReaderDto

Представляет сведения о читателе.

```text
ReaderDto
- id: integer
- full_name: string
- reader_card_number: string
- phone: string | null
- email: string | null
- status: string
```

## 10.4. Контракт CreateLoanRequest

Предназначен для передачи данных при оформлении выдачи.

```text
CreateLoanRequest
- copy_id: integer
- reader_id: integer
- due_date: date
```

## 10.5. Контракт LoanDto

Представляет сведения о выдаче.

```text
LoanDto
- id: integer
- copy_id: integer
- reader_id: integer
- issue_date: datetime
- due_date: date
- return_date: datetime | null
- status: string
```

## 10.6. Обработка ошибок

API должно предусматривать обработку следующих ситуаций:

- книга или экземпляр не найдены;
- читатель не найден;
- экземпляр уже выдан;
- читатель заблокирован;
- дата возврата указана некорректно;
- операция возврата уже выполнена.

Для ошибок следует использовать понятные сообщения и подходящие HTTP-статусы.

Контракты и маршруты являются проектным предложением. Перед использованием их необходимо согласовать с фактической структурой приложения. Если сервер реализован иначе, документацию следует привести в соответствие с реальным API.