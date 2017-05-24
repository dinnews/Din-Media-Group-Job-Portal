
function UpdateEducation(id, profile_id, institution_name, degree_title, field_of_study, education_start_date, education_end_date, education_notes)
{
    $('#institution_name').val(institution_name);
    $('#degree_title').val(degree_title);

    $('#field_of_study').val(field_of_study);
    $('#education_start_date').val(education_start_date);
    $('#education_end_date').val(education_end_date);
    $('#education_notes').val(education_notes);
    $('#institution_name').val(institution_name);
    $('#education_id').val(id);
    $('#education_profile_id').val(profile_id);
    $('#education_request').val("update");


}
function AddEducation() {
    
    $('#education_request_add').val("add");


}
function UpdateExperience(id, profile_id, company_name, job_title, experience_currently_working, experience_start_date, experience_end_date, company_location, experience_notes) {
    $('#company_name').val(company_name);
    $('#job_title').val(job_title);
    $('#experience_currently_working').val(experience_currently_working);
    $('#experience_start_date').val(experience_start_date);
    $('#experience_end_date').val(experience_end_date);
    $('#experience_notes').val(experience_notes);
    $('#company_location').val(company_location);
    $('#experience_id').val(id);
    $('#experience_profile_id').val(profile_id);
    $('#experience_request_update').val("update");
    
}
function AddExperience() {
    
    $('#experience_request_add').val("add");

}

function UpdateProject(id, profile_id, project_title, position, project_currently_working, project_start_date, project_end_date, project_url, project_notes) {
    $('#project_title').val(project_title);
    $('#position').val(position);
    $('#project_currently_working').val(project_currently_working);
    $('#project_start_date').val(project_start_date);
    $('#project_end_date').val(project_end_date);
    $('#project_notes').val(project_notes);
    $('#project_url').val(project_url);
    $('#project_id').val(id);
    $('#project_profile_id').val(profile_id);
    $('#project_request_update').val("update");

}
function AddProject() {
    $('#project_request_add').val("add");

}
function UpdateSkill(id, profile_id, skill_name, skill_experience) {
    $('#skill_name').val(skill_name);
    $('#skill_experience').val(skill_experience);

    $('#skill_id').val(id);
    $('#skill_profile_id').val(profile_id);
    $('#skill_request_update').val("update");

}
function AddSkill() {
    $('#skill_request_add').val("add");

}
function UpdateLanguage(id, profile_id, language_name, language_proficiency) {
    $('#language_name').val(language_name);
    $('#language_proficiency').val(language_proficiency);

    $('#language_id').val(id);
    $('#language_profile_id').val(profile_id);
    $('#language_request_update').val("update");

}
function AddLanguage() {
   
    $('#language_request_add').val("add");

}