from django.contrib.auth.models import User
from rest_framework.test import APITestCase
from rest_framework import status
from company.models import Company
from company.serializers import CompanySerializer
from rest_framework.authtoken.models import Token
from django.urls import reverse

# Test the list method:
# Test when a user is authenticated and companies exist in the database.
# Test when a user is authenticated but no companies exist in the database.
# Test when a user is not authenticated (should return a 401 Unauthorized status).


class CompanyListViewTests(APITestCase):

    def setUp(self):
        # Create a user and obtain a token for authentication
        self.user = User.objects.create_user(
            username='test', email='test@email.com', password='password123'
        )
        self.token = Token.objects.create(user=self.user)

        # Create two test company objects
        self.company1 = Company.objects.create(
            name="Test Company 1", user=self.user)
        self.company2 = Company.objects.create(
            name="Test Company 2", user=self.user)

    def test_list_authenticated_with_companies(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a GET request to list companies
        response = self.client.get('/api/companies/')

        # Assert the status code is 200 OK
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        # Serialize the expected data
        expected_data = CompanySerializer(
            [self.company1, self.company2], many=True).data

        # Assert that the response data matches the expected data
        self.assertEqual(response.data, expected_data)

    def test_list_authenticated_without_companies(self):
        # Delete test companies
        Company.objects.all().delete()

        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a GET request to list companies
        response = self.client.get('/api/companies/')

        # Assert the status code is 200 OK
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        # Assert that the response data is an empty list
        self.assertEqual(response.data, [])

    def test_list_unauthenticated(self):
        # Make a GET request to list companies without authentication
        response = self.client.get('/api/companies/')

        # Assert the status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)


# Test the retrieve method:
# Test when a user is authenticated, and the company with a valid ID exists.
# Test when a user is authenticated, but the company with an invalid ID does not exist (should return a 404 Not Found status).
# Test when a user is not authenticated (should return a 401 Unauthorized status).

class CompanyRetrieveViewTests(APITestCase):

    def setUp(self):
        # Create a user and obtain a token for authentication
        self.user = User.objects.create_user(
            username='test', email='test@email.com', password='password123'
        )
        self.token = Token.objects.create(user=self.user)

        # Create a test company object
        self.company = Company.objects.create(
            name="Test Company", user=self.user)

    def test_retrieve_authenticated(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a GET request to retrieve the company
        response = self.client.get(f'/api/companies/{self.company.id}/')

        # Assert the status code is 200 OK
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        # Serialize the expected data
        expected_data = dict(CompanySerializer(self.company).data)

        # Assert that the response data matches the expected data
        self.assertEqual(response.data, expected_data)

    def test_retrieve_authenticated_nonexistent_company(self):
        # Delete the test company
        self.company.delete()

        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a GET request to retrieve a nonexistent company
        response = self.client.get(f'/api/companies/{self.company.id}/')

        # Assert the status code is 404 Not Found
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)

    def test_retrieve_unauthenticated(self):
        # Make a GET request to retrieve the company without authentication
        response = self.client.get(f'/api/companies/{self.company.id}/')

        # Assert the status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)


# Test the create method:
# Test when a user is authenticated, and valid company data is provided (should return a 201 Created status).
# Test when a user is authenticated, but invalid company data is provided (should return a 400 Bad Request status).
# Test when a user is not authenticated (should return a 401 Unauthorized status).


class CreateCompanyTests(APITestCase):

    def setUp(self):
        # Create a user for authentication
        self.user = User.objects.create_user(
            username='testuser', password='password')
        self.token = Token.objects.create(user=self.user)

    def test_create_authenticated_valid_data(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Define valid company data
        data = {"name": "Company 1", "address": "Address 1", "city": "City 1", "country": "Country 1", "email": "adam@mail.com", "phone": "45651423", "website": "www.company1.com", "description": "Company description",
                "user": 1, "logo": "https://static.vecteezy.com/system/resources/previews/008/214/517/non_2x/abstract-geometric-logo-or-infinity-line-logo-for-your-company-free-vector.jpg"}

        # Send a POST request to create a new company
        url = '/api/companies/'
        response = self.client.post(url, data, format='json')

        # Assert that the response status code is 201 Created
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)

        # Assert that the company object was created in the database
        self.assertEqual(Company.objects.count(), 1)

    def test_create_authenticated_invalid_data(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Define invalid company data (missing required fields)
        data = {
            # Add invalid data here
        }

        # Send a POST request to create a new company with invalid data
        url = '/api/companies/'
        response = self.client.post(url, data, format='json')

        # Assert that the response status code is 400 Bad Request
        self.assertEqual(response.status_code, status.HTTP_400_BAD_REQUEST)

        # Assert that no company object was created in the database
        self.assertEqual(Company.objects.count(), 0)

    def test_create_unauthenticated(self):
        # Define valid company data
        data = {
            "name": "Test Company",
            "address": "Test Address",
            "phone": "1234567890",
            # Add other valid data here
        }

        # Send a POST request to create a new company without authentication
        url = '/api/companies/'
        response = self.client.post(url, data, format='json')

        # Assert that the response status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)

        # Assert that no company object was created in the database
        self.assertEqual(Company.objects.count(), 0)


# Test the update method:
# Test when a user is authenticated, and valid company data is provided for an existing company (should return a 200 OK status).
# Test when a user is authenticated, but invalid company data is provided (should return a 400 Bad Request status).
# Test when a user is authenticated, but the company with the specified ID does not exist (should return a 404 Not Found status).
# Test when a user is not authenticated (should return a 401 Unauthorized status).

class UpdateCompanyTests(APITestCase):
    def setUp(self):
        # Create a user for authentication
        self.user = User.objects.create_user(
            username='testuser', password='password')
        self.token = Token.objects.create(user=self.user)

        data = {"name": "Company 1", "address": "Address 1", "city": "City 1", "country": "Country 1", "email": "adam@mail.com", "phone": "45651423", "website": "www.company1.com", "description": "Company description",
                "user": 1, "logo": "https://static.vecteezy.com/system/resources/previews/008/214/517/non_2x/abstract-geometric-logo-or-infinity-line-logo-for-your-company-free-vector.jpg"}

        # Create a company for testing
        self.company = Company.objects.create(
            name="Test Company",
            address="Test Address",
            phone="1234567890",
            email="adam@mail.com",
            website="www.testcompany.com",
            description="Test Description",
            user=self.user,
            logo="https://static.vecteezy.com/system/resources/previews/008/214/517/non_2x/abstract-geometric-logo-or-infinity-line-logo-for-your-company-free-vector.jpg"
        )

    def test_update_authenticated_valid_data(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Define valid company data for update
        data = {
            "name": "Updated Company Name",
            "address": "Updated Address",
            # Add other valid data here
        }

        # Send a PUT request to update an existing company
        url = f'/api/companies/{self.company.pk}/'
        response = self.client.put(url, data, format='json')

        # Assert that the response status code is 200 OK
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        # Refresh the company object from the database
        self.company.refresh_from_db()

        # Assert that the company data has been updated
        self.assertEqual(self.company.name, "Updated Company Name")
        self.assertEqual(self.company.address, "Updated Address")

    def test_update_authenticated_invalid_data(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Define invalid company data (missing required fields)
        data = {
            "invalid_column": 'wrong data'
        }

        # Send a PUT request to update an existing company with invalid data
        url = f'/api/companies/{self.company.pk}/'
        response = self.client.put(url, data, format='json')

        # Assert that the response status code is 400 Bad Request
        self.assertEqual(response.status_code, status.HTTP_400_BAD_REQUEST)

        # Refresh the company object from the database
        self.company.refresh_from_db()

        # Assert that the company data remains unchanged
        self.assertEqual(self.company.name, "Test Company")
        self.assertEqual(self.company.address, "Test Address")

    def test_update_authenticated_nonexistent_company(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Define valid company data for update
        data = {
            "name": "Updated Company Name",
            "address": "Updated Address",
            # Add other valid data here
        }

        # Send a PUT request to update a nonexistent company
        nonexistent_company_id = self.company.pk + 1  # An ID that does not exist
        url = f'/api/companies/{nonexistent_company_id}/'
        response = self.client.put(url, data, format='json')

        # Assert that the response status code is 404 Not Found
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)

    def test_update_unauthenticated(self):
        # Define valid company data for update
        data = {
            "name": "Updated Company Name",
            "address": "Updated Address",
            # Add other valid data here
        }

        # Send a PUT request to update an existing company without authentication
        url = f'/api/companies/{self.company.pk}/'
        response = self.client.put(url, data, format='json')

        # Assert that the response status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)

        # Refresh the company object from the database
        self.company.refresh_from_db()

        # Assert that the company data remains unchanged
        self.assertEqual(self.company.name, "Test Company")
        self.assertEqual(self.company.address, "Test Address")


# Test the destroy method:
# Test when a user is authenticated, and the company with a valid ID exists (should return a 204 No Content status).
# Test when a user is authenticated, but the company with the specified ID does not exist (should return a 404 Not Found status).
# Test when a user is not authenticated (should return a 401 Unauthorized status).

class CompanyDestroyViewTests(APITestCase):
    def setUp(self):
        # Create a user
        self.user = User.objects.create_user(
            username='testuser',
            password='testpassword'
        )

        # Create an authentication token for the user
        self.token = Token.objects.create(user=self.user)

        # Create a company
        self.company = Company.objects.create(
            name='Test Company',
            address='Test Address',
            phone='1234567890',
            email='test@example.com',
            website='www.example.com',
            description='Test Description',
            logo='test_logo.png',
            user=self.user
        )

    def test_destroy_authenticated_valid_company(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a DELETE request to destroy the company
        response = self.client.delete(f'/api/companies/{self.company.id}/')

        # Assert the status code is 204 No Content
        self.assertEqual(response.status_code, status.HTTP_204_NO_CONTENT)

        # Check if the company has been deleted from the database
        self.assertFalse(Company.objects.filter(id=self.company.id).exists())

    def test_destroy_authenticated_nonexistent_company(self):
        # Authenticate with the token
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token.key)

        # Make a DELETE request with an invalid company ID
        invalid_company_id = self.company.id + 1
        response = self.client.delete(f'/api/companies/{invalid_company_id}/')

        # Assert the status code is 404 Not Found
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)

    def test_destroy_unauthenticated(self):
        # Make a DELETE request without authentication
        response = self.client.delete(f'/api/companies/{self.company.id}/')

        # Assert the status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)


# Test the getCompanyByUser action:
# Test when a user is authenticated, and there are companies associated with the user (should return a 200 OK status).
# Test when a user is authenticated, but there are no companies associated with the user (should return a 404 Not Found status).
# Test when a user is not authenticated (should return a 401 Unauthorized status).

class CompanyGetCompanyByUserViewTests(APITestCase):
    def setUp(self):
        # Create two users
        self.user1 = User.objects.create_user(
            username='user1',
            password='password1'
        )
        self.user2 = User.objects.create_user(
            username='user2',
            password='password2'
        )

        # Create authentication tokens for the users
        self.token1 = Token.objects.create(user=self.user1)
        self.token2 = Token.objects.create(user=self.user2)

        # Create companies associated with user1
        self.company1 = Company.objects.create(
            name='Company 1',
            address='Address 1',
            phone='1234567890',
            email='company1@example.com',
            website='www.company1.com',
            description='Description 1',
            logo='company1_logo.png',
            user=self.user1
        )
        self.company2 = Company.objects.create(
            name='Company 2',
            address='Address 2',
            phone='9876543210',
            email='company2@example.com',
            website='www.company2.com',
            description='Description 2',
            logo='company2_logo.png',
            user=self.user1
        )

    def test_get_company_by_user_authenticated_with_companies(self):
        # Authenticate with the token of user1
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token1.key)

        # Make a GET request to get companies by user
        response = self.client.get(f'/api/companies/byUser/{self.user1.id}/')

        # Assert the status code is 200 OK
        self.assertEqual(response.status_code, status.HTTP_200_OK)

        # Convert response data to a list of dictionaries and filter out unnecessary fields
        response_data = [{'id': item['id'], 'name': item['name']}
                         for item in response.data]

        # Check if the serialized data contains both companies of user1
        serialized_data = [{'id': self.company1.id, 'name': self.company1.name}, {
            'id': self.company2.id, 'name': self.company2.name}]

        self.assertEqual(response_data, serialized_data)

    def test_get_company_by_user_authenticated_no_companies(self):
        # Authenticate with the token of user2 (no companies associated with user2)
        self.client.credentials(HTTP_AUTHORIZATION='Token ' + self.token2.key)

        # Make a GET request to get companies by user
        response = self.client.get(f'/api/companies/byUser/{self.user2.id}/')

        # Assert the status code is 404 Not Found
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)

    def test_get_company_by_user_unauthenticated(self):
        # Make a GET request without authentication
        response = self.client.get(f'/api/companies/byUser/{self.user1.id}/')

        # Assert the status code is 401 Unauthorized
        self.assertEqual(response.status_code, status.HTTP_401_UNAUTHORIZED)
