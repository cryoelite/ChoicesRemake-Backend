#!/bin/bash
set -e

# Install necessary dependencies
sudo apt-get update -y
sudo DEBIAN_FRONTEND=noninteractive apt-get -y -o Dpkg::Options::="--force-confdef" -o Dpkg::Options::="--force-confold" dist-upgrade
sudo apt-get update -y
sudo apt-get install unzip -y
sudo apt-get -y -qq install curl wget git vim apt-transport-https ca-certificates

#setup docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose

#setup CR
wget https://codeload.github.com/cryoelite/ChoicesRemake-Backend/zip/refs/heads/main -O cr.zip
chmod 0777 cr.zip
unzip cr.zip
cd ChoicesRemake-Backend-main/Provisioner/Packer-AWS/
sudo sh dockerStart.sh

# Setup sudo to allow no-password sudo for "cryoelite" group and adding millify" user
sudo groupadd -r cryoelite
sudo useradd -m -s /bin/bash millify
sudo usermod -a -G cryoelite millify
sudo cp /etc/sudoers /etc/sudoers.orig
echo "millify ALL=(ALL) NOPASSWD:ALL" | sudo tee /etc/sudoers.d/millify

# Installing SSH key
sudo mkdir -p /home/millify/.ssh
sudo chmod 700 /home/millify/.ssh
sudo cp /tmp/tfPublicKey.pub /home/millify/.ssh/authorized_keys
sudo chmod 600 /home/millify/.ssh/authorized_keys
sudo chown -R millify /home/millify/.ssh
sudo usermod --shell /bin/bash millify

