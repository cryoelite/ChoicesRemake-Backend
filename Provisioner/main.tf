terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
      version = "~> 2.13.0"
    }
    caddy = {
      source = "conradludgate/caddy"
      version = "0.2.8"
    }
    oci = {
      source = "hashicorp/oci"
      version = "4.55.0"
    }
  }
    backend "remote" {
    organization = "cryoelite"
    workspaces {
      name = "ChoicesRemake"
    }
  }
}

provider "docker" {

  
}

provider "oci" {
  # Configuration options
}

provider "caddy" {

}

