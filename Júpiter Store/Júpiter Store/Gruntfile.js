module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        sass: {
            dist: {
                options: {
                    sourcemap: "file"
                },
                files: [{
                    expand: true,
                    cwd: 'Content/sass',
                    src: ['{,*/}*.scss'],
                    dest: 'Content/css',
                    ext: '.css'
                }]
            }
        },

        watch: {
            sass: {
                files: 'Content/sass/{,**}*.scss',
                tasks: ['sass']
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['sass', 'watch']);

};