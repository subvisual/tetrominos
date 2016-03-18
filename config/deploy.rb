set :repo_url, 'git@github-subvisual.co:subvisual/tetrominos'
set :application, 'tetrominos.subvisual.co'
set :stage, :production
set :branch, (ENV['DEPLOY_BRANCH'] || :site)

set :linked_dirs, %w(log)

set :format, :pretty
set :log_level, :debug
set :pty, true
set :scm, :middleman
