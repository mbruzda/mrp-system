import docker


def pull_and_run_mrp_container():
    published_ports = {'80/tcp': 80,
                       '443/tcp': 443}

    client = docker.from_env()
    client.images.pull('mrocin/mrp-api')
    client.containers.run(image='mrocin/mrp-api',
                          ports=published_ports,
                          name='mrp-api',
                          detach=True)


def kill_mrp_container():
    client = docker.from_env()
    container = client.containers.get("mrp-api")
    container.kill()
    container.remove()

