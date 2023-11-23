from django.db import models

# Create your models here.


class Company(models.Model):
    id = models.AutoField(primary_key=True)
    name = models.CharField(max_length=100)
    address = models.CharField(max_length=100)
    phone = models.CharField(max_length=10)
    email = models.EmailField()
    website = models.CharField(max_length=100)
    description = models.CharField(max_length=100)
    logo = models.CharField(max_length=500)
    user = models.ForeignKey('auth.User', on_delete=models.CASCADE)

    class Meta:
        db_table = "python_company"

    def __str__(self):
        return self.name
