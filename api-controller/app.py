from flask import Flask, request, jsonify
from docker_utils import pull_and_run_mrp_container, kill_mrp_container

app = Flask(__name__)

success = {'status': 200, 'message': 'docker container restarted'}


@app.get('/restart')
def restart():
    try:
        kill_mrp_container()
    except Exception as e:
        print(e)
    pull_and_run_mrp_container()
    return jsonify(success)
