# GenerateRandomPeopleImg
# PowerShell script to download employee photos
# This script downloads random employee photos from randomuser.me and you can only download 100 images
# Ensure you have the necessary permissions to run this script
# Set the base path where the photos will be stored
# Make sure to change the path to your actual solution path
# Alter table to include a column for Employee Photo
# HumanResources.Employee table has 290 Employees

$basePath = "C:Your path here\GenerateRandomPeopleImg\GenerateRandomPeopleImg\EmployeePhotos"

# Create male and female folders
New-Item -Path "$basePath\M" -ItemType Directory -Force
New-Item -Path "$basePath\F" -ItemType Directory -Force

# Download 206 male photos
For ($i = 1; $i -le 100; $i++) {
    $url = "https://randomuser.me/api/portraits/men/$i.jpg"
    $outFile = "$basePath\M\$i.jpg"
    Invoke-WebRequest -Uri $url -OutFile $outFile
}

# Download 84 female photos
For ($i = 1; $i -le 100; $i++) {
    $url = "https://randomuser.me/api/portraits/women/$i.jpg"
    $outFile = "$basePath\F\$i.jpg"
    Invoke-WebRequest -Uri $url -OutFile $outFile
}

# 🔁 Duplicate and rename to reach 200+ male photos (M\100.jpg to M\210.jpg)
for ($j = 100; $j -lt 210; $j++) {
    $source = "$basePath\M\$(Get-Random -Minimum 0 -Maximum 100).jpg"
    $target = "$basePath\M\$j.jpg"
    Copy-Item -Path $source -Destination $target
}

Write-Output "Photos downloaded to $basePath"
