<template>
    <div class="demo-content">
        <Steps :current="currentStepIndex">
            <Step title="基本信息" icon="md-card"></Step>
            <Step title="表单设计" icon="md-paper"></Step>
            <Step title="流程设计" icon="logo-steam"></Step>
        </Steps>

        <WorkflowNomals
            v-show="currentStepIndex == 0"
            v-model="entity"
            class="demo-wf margin-top-10"
            @on-validated="setValidated"
        />
        <div v-show="currentStepIndex == 1" class="demo-wf margin-top-10">
            <FormDesign
                v-if="currentStepIndex == 1"
                v-model="entity.inputs"
                @on-validated="setValidated"
            />
        </div>
        <div v-show="currentStepIndex == 2" class="demo-wf margin-top-10">
            <WorkflowDesign
                v-if="currentStepIndex == 2"
                v-model="entity.nodes"
                :inputs="entity.inputs"
            />
        </div>

        <div class="demo-tool">
            <Button
                type="info"
                size="large"
                icon="md-arrow-round-back"
                :disabled="currentStepIndex == 0"
                @click="preStep"
            >
                {{ L("Pre Step") }}
            </Button>
            <Button
                v-if="currentStepIndex != 2"
                type="info"
                size="large"
                icon="md-arrow-round-forward"
                @click="nextStep"
            >
                {{ L("Next Step") }}
            </Button>
            <Button
                v-if="currentStepIndex == 2"
                type="success"
                size="large"
                style="width: 120px"
                @click="Completed"
            >
                {{ L("Completed") }}
            </Button>
        </div>
        <Modal
            v-model="saveSuccessed"
            :closable="false"
            :mask-closable="false"
            :footer-hide="true"
        >
            <div class="ivu-modal-confirm">
                <div class="ivu-modal-confirm-head">
                    <div
                        class="
                            ivu-modal-confirm-head-icon
                            ivu-modal-confirm-head-icon-success
                        "
                    >
                        <i class="ivu-icon ivu-icon-ios-checkmark-circle"></i>
                    </div>
                    <div class="ivu-modal-confirm-head-title">
                        {{ L("Save Successful!") }}
                    </div>
                </div>
                <div class="ivu-modal-confirm-body">
                    <div>
                        <p>{{ L("Whether to continue to add?") }}</p>
                    </div>
                </div>
                <div class="ivu-modal-confirm-footer">
                    <Button type="primary" @click="reset">
                        {{ L("Continue to add") }}
                    </Button>
                    <Button type="default" @click="returnToList">
                        {{ L("Return to list") }}
                    </Button>
                </div>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
import { Component } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";
import WorkflowDefinition from "@/store/entities/workflow-definition";
import WorkflowNomals from "./workflow-nomal.vue";
import WorkflowDesign from "./workflow-design.vue";
import FormDesign from "./workflow-form-design.vue";

@Component({
    components: {
        WorkflowDesign,
        FormDesign,
        WorkflowNomals,
    },
})
export default class WorkFlowCreate extends AbpBase {
    currentStepIndex = 0;
    currentStepValidated = false;
    saveSuccessed = false;
    entity: WorkflowDefinition = new WorkflowDefinition();

    nextStep() {
        this.currentStepIndex++;
        this.currentStepValidated = false;
    }

    preStep() {
        this.currentStepIndex--;
        this.currentStepValidated = true;
    }

    setValidated(v) {
        this.currentStepValidated = v;
    }

    returnToList() {
        this.$store.commit("app/removeTag", this.$route.name);
        this.$router.push({
            name: "workflowlist",
        });
    }
    reset() {
        this.currentStepIndex = 0;
        this.saveSuccessed = false;
        this.entity = new WorkflowDefinition();
    }

    async Completed() {
        await this.$store.dispatch({
            type: "workflow/create",
            data: this.entity,
        });
        this.saveSuccessed = true;
    }
}
</script>

<style scoped>
.demo-content {
    height: 100%;
    position: relative;
}

.demo-wf {
    height: 700px;
    position: relative;
}

.demo-tool {
    margin-top: 20px;
    width: 100%;
    text-align: center;
}
.ivu-steps {
    background: #fff;
    padding: 10px;
}
.demo-tool .ivu-btn {
    margin-left: 10px;
}

.wf-btn {
    position: absolute !important;
}
</style>