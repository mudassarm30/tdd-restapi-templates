from django.urls import re_path, path, include
from django.contrib import admin

from . import views

user_urls = [
    path('signup', views.signup, name='signup'),
    path('login', views.login, name='login'),
    path('test_token', views.test_token, name='test_token'),
]

urlpatterns = [

    path('admin/', admin.site.urls),
    path('api/users/', include(user_urls)),
    path('api/companies/', include('company.urls')),
]
