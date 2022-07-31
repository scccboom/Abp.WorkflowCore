<template>
    <div>
        <Modal
            :title="L(workflow.title)"
            :mask-closable="false"
            :value="value"
            @on-ok="save"
            @on-visible-change="visibleChange"
        >
            <Form
                ref="flowForm"
                label-position="top"
                :rules="flowRules"
                :model="formData"
            >
                <Row
                    :gutter="15"
                    v-for="(row, rowkey) in workflow.inputs"
                    :key="rowkey"
                >
                    <Col
                        v-for="(column, key) in row"
                        :key="key"
                        :span="24 / row.length"
                    >
                        <WorkflowFormItem
                            v-for="(element, key) in column"
                            v-model="formData[element.name]"
                            :element="column[key]"
                            :key="element.id"
                        />
                    </Col>
                </Row>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{ L("Cancel") }}</Button>
                <Button type="primary" @click="save">{{ L("OK") }}</Button>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
import { Component, Prop } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";
import WorkflowDefinition from "@/store/entities/workflow-definition";
import WorkflowFormItem from "./workflow-form-item.vue";

@Component({
    components: {
        WorkflowFormItem,
    },
})
export default class StartWorkflow extends AbpBase {
    @Prop({ type: Boolean, default: false })
    readonly value: boolean;

    @Prop({ type: Object, default: null })
    readonly workflow: WorkflowDefinition;

    flowRules = {};
    formData = {};

    get ruleDefinitions() {
        let rdef = {};
        this.$store.state.workflow.rules.forEach((element) => {
            rdef[element.name] = element.value;
        });
        return rdef;
    }

    get workflowInputs() {
        let arr = [];
        this.workflow.inputs.forEach((element) => {
            element.forEach((obj) => {
                arr.push(...obj);
            });
        });
        return arr;
    }

    save() {
        (this.$refs.flowForm as any).validate(async (valid: boolean) => {
            if (valid) {
                await this.$store.dispatch({
                    type: "workflow/start",
                    data: {
                        id: this.workflow.id,
                        version: this.workflow.version,
                        inputs: this.formData,
                    },
                });
                (this.$refs.flowForm as any).resetFields();
                this.$emit("save-success");
                this.$emit("input", false);
            }
        });
    }

    cancel() {
        (this.$refs.flowForm as any).resetFields();
        this.$emit("input", false);
    }

    visibleChange(value: boolean) {
        if (!value) {
            this.$emit("input", value);
            return;
        }
        let rules = {};
        let form = {};
        this.workflowInputs.forEach((i) => {
            rules[i.name] = [];
            form[i.name] = null;
            if (i.rules && i.rules.length > 0) {
                i.rules.forEach((r) => {
                    rules[i.name].push(this.ruleDefinitions[r]);
                });
            }
        });
        this.flowRules = { ...rules };
        this.formData = { ...form };
    }
}
</script>

