var skillCount = 0;
var skillsContainer = document.getElementById('skillsContainer');
var addSkillButton = document.getElementById('addSkillButton');

addSkillButton.addEventListener('click', function () {
    // Create a new Skill input field and append it to the container
    var skillInput = document.createElement('input');
    skillInput.type = 'text';
    skillInput.name = 'Skills[' + skillCount + '].Name';
    skillInput.placeholder = 'Skill Name';
    skillsContainer.appendChild(skillInput);

    // Create a hidden input for Skill GUID
    var guidInput = document.createElement('input');
    guidInput.type = 'hidden';
    guidInput.name = 'Skills[' + skillCount + '].Guid';
    guidInput.value = generateGuid(); // You need to implement generateGuid function
    skillsContainer.appendChild(guidInput);

    skillCount++;
});