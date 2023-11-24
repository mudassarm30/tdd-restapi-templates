# RestApi Project in Python and Django

This project is created in Django (Python) using code first approach. It takes the test driven approach and contains CICD using Github Actions that deploys the project in AWS API Gateway and AWS Lambda Function.

## Files in the project

- `tests.py`: This file contains the tests for the project. It includes tests for different scenarios such as when a user is authenticated and companies exist in the database, when a user is authenticated but no companies exist in the database, and when a user is not authenticated.

- `models.py`: This file contains the data models for the project.

- `serializers.py`: This file is responsible for converting complex data types, such as queryset and model instances, to Python native datatypes that can then be easily rendered into JSON, XML, or other content types.

- `views.py`: This file contains the views for the project. It is responsible for what content is displayed on a given page when a user navigates to a certain URL.

- `urls.py`: This file contains all of the URL configurations for the project.

## Installation

Instructions on how to install the project, for example:

```bash
pip install -r requirements.txt

```

# Project Title

A brief description of what this project does and who it's for.

## Files in the project

- `tests.py`: This file contains the tests for the project. It includes tests for different scenarios such as when a user is authenticated and companies exist in the database, when a user is authenticated but no companies exist in the database, and when a user is not authenticated.

- `models.py`: This file contains the data models for the project.

- `serializers.py`: This file is responsible for converting complex data types, such as queryset and model instances, to Python native datatypes that can then be easily rendered into JSON, XML, or other content types.

- `views.py`: This file contains the views for the project. It is responsible for what content is displayed on a given page when a user navigates to a certain URL.

- `urls.py`: This file contains all of the URL configurations for the project.

## Installation

Instructions on how to install the project, for example:

```bash
pip install -r requirements.txt
```

## Usage

Instructions on how to use the project, for example:

```bash
python manage.py runserver
```

## Running Tests

To run the tests, use the following command:

```bash
python manage.py test
```

## Adding a New Module

In Django, a module is referred to as an "app". Here are the steps to add a new app to your project:

1. First, navigate to the root directory of your project in the terminal.

2. Run the following command to create a new app. Replace `new_module` with the name of your app:

```bash
python manage.py startapp new_module
```

3. This will create a new directory named `new_module` with the structure of a Django app.

4. Now, you need to add the new app to the `INSTALLED_APPS` list in your settings file (`settings.py` or `base.py` depending on your project structure). It should look something like this:

```python
INSTALLED_APPS = [
    ...
    'new_module',
    ...
]
```

5. Now you can start building your new module! Remember to create a `models.py` file for your database schema, a `views.py` file for your app views, and a `urls.py` file to handle routing. You can also create a `tests.py` file to write tests for your app.

6. After creating your models, remember to apply the migrations:

```bash
python manage.py makemigrations new_module
python manage.py migrate
```

7. If your app has its own `urls.py`, include it in the project's main `urls.py`:

```python
from django.urls import include

urlpatterns = [
    ...
    path('new_module/', include('new_module.urls')),
    ...
]
```

Remember to replace `new_module` with the name of your app.

## Contributing

Details about how to contribute to the project.

## License

Information about the project's license (if applicable).

## Contact

Your contact information so people can reach out to you.
```